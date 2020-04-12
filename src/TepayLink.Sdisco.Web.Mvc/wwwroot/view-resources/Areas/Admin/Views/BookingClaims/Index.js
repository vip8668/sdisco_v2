(function () {
    $(function () {

        var _$bookingClaimsTable = $('#BookingClaimsTable');
        var _bookingClaimsService = abp.services.app.bookingClaims;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.BookingClaims.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.BookingClaims.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.BookingClaims.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingClaims/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BookingClaims/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBookingClaimModal'
        });

		 var _viewBookingClaimModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingClaims/ViewbookingClaimModal',
            modalClass: 'ViewBookingClaimModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$bookingClaimsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bookingClaimsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BookingClaimsTableFilter').val(),
					minBookingDetailIdFilter: $('#MinBookingDetailIdFilterId').val(),
					maxBookingDetailIdFilter: $('#MaxBookingDetailIdFilterId').val(),
					claimReasonTitleFilter: $('#ClaimReasonTitleFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewBookingClaimModal.open({ id: data.record.bookingClaim.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.bookingClaim.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBookingClaim(data.record.bookingClaim);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bookingClaim.bookingDetailId",
						 name: "bookingDetailId"   
					},
					{
						targets: 2,
						 data: "claimReasonTitle" ,
						 name: "claimReasonFk.title" 
					}
            ]
        });

        function getBookingClaims() {
            dataTable.ajax.reload();
        }

        function deleteBookingClaim(bookingClaim) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bookingClaimsService.delete({
                            id: bookingClaim.id
                        }).done(function () {
                            getBookingClaims(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewBookingClaimButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _bookingClaimsService
                .getBookingClaimsToExcel({
				filter : $('#BookingClaimsTableFilter').val(),
					minBookingDetailIdFilter: $('#MinBookingDetailIdFilterId').val(),
					maxBookingDetailIdFilter: $('#MaxBookingDetailIdFilterId').val(),
					claimReasonTitleFilter: $('#ClaimReasonTitleFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBookingClaimModalSaved', function () {
            getBookingClaims();
        });

		$('#GetBookingClaimsButton').click(function (e) {
            e.preventDefault();
            getBookingClaims();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBookingClaims();
		  }
		});
    });
})();