(function () {
    $(function () {

        var _$claimReasonsTable = $('#ClaimReasonsTable');
        var _claimReasonsService = abp.services.app.claimReasons;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ClaimReasons.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ClaimReasons.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ClaimReasons.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ClaimReasons/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ClaimReasons/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditClaimReasonModal'
        });

		 var _viewClaimReasonModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ClaimReasons/ViewclaimReasonModal',
            modalClass: 'ViewClaimReasonModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$claimReasonsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _claimReasonsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ClaimReasonsTableFilter').val(),
					titleFilter: $('#TitleFilterId').val()
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
                                    _viewClaimReasonModal.open({ id: data.record.claimReason.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.claimReason.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteClaimReason(data.record.claimReason);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "claimReason.title",
						 name: "title"   
					}
            ]
        });

        function getClaimReasons() {
            dataTable.ajax.reload();
        }

        function deleteClaimReason(claimReason) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _claimReasonsService.delete({
                            id: claimReason.id
                        }).done(function () {
                            getClaimReasons(true);
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

        $('#CreateNewClaimReasonButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _claimReasonsService
                .getClaimReasonsToExcel({
				filter : $('#ClaimReasonsTableFilter').val(),
					titleFilter: $('#TitleFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditClaimReasonModalSaved', function () {
            getClaimReasons();
        });

		$('#GetClaimReasonsButton').click(function (e) {
            e.preventDefault();
            getClaimReasons();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getClaimReasons();
		  }
		});
    });
})();