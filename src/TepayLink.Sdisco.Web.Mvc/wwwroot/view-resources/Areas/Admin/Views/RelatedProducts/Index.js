(function () {
    $(function () {

        var _$relatedProductsTable = $('#RelatedProductsTable');
        var _relatedProductsService = abp.services.app.relatedProducts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.RelatedProducts.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.RelatedProducts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.RelatedProducts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RelatedProducts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/RelatedProducts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRelatedProductModal'
        });

		 var _viewRelatedProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RelatedProducts/ViewrelatedProductModal',
            modalClass: 'ViewRelatedProductModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$relatedProductsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _relatedProductsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RelatedProductsTableFilter').val(),
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
                                    _viewRelatedProductModal.open({ id: data.record.relatedProduct.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.relatedProduct.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRelatedProduct(data.record.relatedProduct);
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
						 name: "relatedProductFk.name" 
					}
            ]
        });

        function getRelatedProducts() {
            dataTable.ajax.reload();
        }

        function deleteRelatedProduct(relatedProduct) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _relatedProductsService.delete({
                            id: relatedProduct.id
                        }).done(function () {
                            getRelatedProducts(true);
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

        $('#CreateNewRelatedProductButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _relatedProductsService
                .getRelatedProductsToExcel({
				filter : $('#RelatedProductsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					productName2Filter: $('#ProductName2FilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRelatedProductModalSaved', function () {
            getRelatedProducts();
        });

		$('#GetRelatedProductsButton').click(function (e) {
            e.preventDefault();
            getRelatedProducts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRelatedProducts();
		  }
		});
    });
})();