(function () {
    $(function () {

        var _$saveItemsTable = $('#SaveItemsTable');
        var _saveItemsService = abp.services.app.saveItems;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.SaveItems.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.SaveItems.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.SaveItems.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SaveItems/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/SaveItems/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSaveItemModal'
        });

		 var _viewSaveItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SaveItems/ViewsaveItemModal',
            modalClass: 'ViewSaveItemModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$saveItemsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _saveItemsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#SaveItemsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val()
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
                                    _viewSaveItemModal.open({ id: data.record.saveItem.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.saveItem.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteSaveItem(data.record.saveItem);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getSaveItems() {
            dataTable.ajax.reload();
        }

        function deleteSaveItem(saveItem) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _saveItemsService.delete({
                            id: saveItem.id
                        }).done(function () {
                            getSaveItems(true);
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

        $('#CreateNewSaveItemButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _saveItemsService
                .getSaveItemsToExcel({
				filter : $('#SaveItemsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSaveItemModalSaved', function () {
            getSaveItems();
        });

		$('#GetSaveItemsButton').click(function (e) {
            e.preventDefault();
            getSaveItems();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getSaveItems();
		  }
		});
    });
})();