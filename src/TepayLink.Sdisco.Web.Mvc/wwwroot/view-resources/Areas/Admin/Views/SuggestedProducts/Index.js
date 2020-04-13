(function () {
    $(function () {

        var _$suggestedProductsTable = $('#SuggestedProductsTable');
        var _suggestedProductsService = abp.services.app.suggestedProducts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.SuggestedProducts.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.SuggestedProducts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.SuggestedProducts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SuggestedProducts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/SuggestedProducts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSuggestedProductModal'
        });

		 var _viewSuggestedProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SuggestedProducts/ViewsuggestedProductModal',
            modalClass: 'ViewSuggestedProductModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$suggestedProductsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _suggestedProductsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#SuggestedProductsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					productName2Filter: $('#ProductName2FilterId').val()
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
                                    _viewSuggestedProductModal.open({ id: data.record.suggestedProduct.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.suggestedProduct.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteSuggestedProduct(data.record.suggestedProduct);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productName" ,
						 name: "productFk.name" 
					},
					{
						targets: 2,
						 data: "productName2" ,
						 name: "suggestedProductFk.name" 
					}
            ]
        });

        function getSuggestedProducts() {
            dataTable.ajax.reload();
        }

        function deleteSuggestedProduct(suggestedProduct) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _suggestedProductsService.delete({
                            id: suggestedProduct.id
                        }).done(function () {
                            getSuggestedProducts(true);
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

        $('#CreateNewSuggestedProductButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _suggestedProductsService
                .getSuggestedProductsToExcel({
				filter : $('#SuggestedProductsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					productName2Filter: $('#ProductName2FilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSuggestedProductModalSaved', function () {
            getSuggestedProducts();
        });

		$('#GetSuggestedProductsButton').click(function (e) {
            e.preventDefault();
            getSuggestedProducts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getSuggestedProducts();
		  }
		});
    });
})();