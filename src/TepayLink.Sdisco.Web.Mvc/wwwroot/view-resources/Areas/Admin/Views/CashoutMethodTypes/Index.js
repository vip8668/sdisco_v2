(function () {
    $(function () {

        var _$cashoutMethodTypesTable = $('#CashoutMethodTypesTable');
        var _cashoutMethodTypesService = abp.services.app.cashoutMethodTypes;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.CashoutMethodTypes.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.CashoutMethodTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.CashoutMethodTypes.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/CashoutMethodTypes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/CashoutMethodTypes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCashoutMethodTypeModal'
        });

		 var _viewCashoutMethodTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/CashoutMethodTypes/ViewcashoutMethodTypeModal',
            modalClass: 'ViewCashoutMethodTypeModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$cashoutMethodTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _cashoutMethodTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CashoutMethodTypesTableFilter').val(),
					titleFilter: $('#TitleFilterId').val(),
					noteFilter: $('#NoteFilterId').val()
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
                                    _viewCashoutMethodTypeModal.open({ id: data.record.cashoutMethodType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.cashoutMethodType.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCashoutMethodType(data.record.cashoutMethodType);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "cashoutMethodType.title",
						 name: "title"   
					},
					{
						targets: 2,
						 data: "cashoutMethodType.note",
						 name: "note"   
					}
            ]
        });

        function getCashoutMethodTypes() {
            dataTable.ajax.reload();
        }

        function deleteCashoutMethodType(cashoutMethodType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _cashoutMethodTypesService.delete({
                            id: cashoutMethodType.id
                        }).done(function () {
                            getCashoutMethodTypes(true);
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

        $('#CreateNewCashoutMethodTypeButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _cashoutMethodTypesService
                .getCashoutMethodTypesToExcel({
				filter : $('#CashoutMethodTypesTableFilter').val(),
					titleFilter: $('#TitleFilterId').val(),
					noteFilter: $('#NoteFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCashoutMethodTypeModalSaved', function () {
            getCashoutMethodTypes();
        });

		$('#GetCashoutMethodTypesButton').click(function (e) {
            e.preventDefault();
            getCashoutMethodTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCashoutMethodTypes();
		  }
		});
    });
})();