(function () {
    $(function () {

        var _$bookingRefundsTable = $('#BookingRefundsTable');
        var _bookingRefundsService = abp.services.app.bookingRefunds;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.BookingRefunds.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.BookingRefunds.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.BookingRefunds.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingRefunds/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BookingRefunds/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBookingRefundModal'
        });

		 var _viewBookingRefundModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingRefunds/ViewbookingRefundModal',
            modalClass: 'ViewBookingRefundModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$bookingRefundsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bookingRefundsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BookingRefundsTableFilter').val(),
					minBookingDetailIdFilter: $('#MinBookingDetailIdFilterId').val(),
					maxBookingDetailIdFilter: $('#MaxBookingDetailIdFilterId').val(),
					minRefundMethodIdFilter: $('#MinRefundMethodIdFilterId').val(),
					maxRefundMethodIdFilter: $('#MaxRefundMethodIdFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val()
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
                                    _viewBookingRefundModal.open({ id: data.record.bookingRefund.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.bookingRefund.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBookingRefund(data.record.bookingRefund);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bookingRefund.bookingDetailId",
						 name: "bookingDetailId"   
					},
					{
						targets: 2,
						 data: "bookingRefund.refundMethodId",
						 name: "refundMethodId"   
					},
					{
						targets: 3,
						 data: "bookingRefund.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "bookingRefund.status",
						 name: "status"   
					},
					{
						targets: 5,
						 data: "bookingRefund.amount",
						 name: "amount"   
					}
            ]
        });

        function getBookingRefunds() {
            dataTable.ajax.reload();
        }

        function deleteBookingRefund(bookingRefund) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bookingRefundsService.delete({
                            id: bookingRefund.id
                        }).done(function () {
                            getBookingRefunds(true);
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

        $('#CreateNewBookingRefundButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _bookingRefundsService
                .getBookingRefundsToExcel({
				filter : $('#BookingRefundsTableFilter').val(),
					minBookingDetailIdFilter: $('#MinBookingDetailIdFilterId').val(),
					maxBookingDetailIdFilter: $('#MaxBookingDetailIdFilterId').val(),
					minRefundMethodIdFilter: $('#MinRefundMethodIdFilterId').val(),
					maxRefundMethodIdFilter: $('#MaxRefundMethodIdFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBookingRefundModalSaved', function () {
            getBookingRefunds();
        });

		$('#GetBookingRefundsButton').click(function (e) {
            e.preventDefault();
            getBookingRefunds();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBookingRefunds();
		  }
		});
    });
})();