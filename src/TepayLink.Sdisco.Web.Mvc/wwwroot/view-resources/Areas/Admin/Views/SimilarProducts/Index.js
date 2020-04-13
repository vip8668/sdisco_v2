(function () {
    $(function () {

        var _$similarProductsTable = $('#SimilarProductsTable');
        var _similarProductsService = abp.services.app.similarProducts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.SimilarProducts.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.SimilarProducts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.SimilarProducts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SimilarProducts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/SimilarProducts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSimilarProductModal'
        });

		 var _viewSimilarProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SimilarProducts/ViewsimilarProductModal',
            modalClass: 'ViewSimilarProductModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$similarProductsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _similarProductsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#SimilarProductsTableFilter').val(),
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
                                    _viewSimilarProductModal.open({ id: data.record.similarProduct.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.similarProduct.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteSimilarProduct(data.record.similarProduct);
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
						 name: "similarProductFk.name" 
					}
            ]
        });

        function getSimilarProducts() {
            dataTable.ajax.reload();
        }

        function deleteSimilarProduct(similarProduct) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _similarProductsService.delete({
                            id: similarProduct.id
                        }).done(function () {
                            getSimilarProducts(true);
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

        $('#CreateNewSimilarProductButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _similarProductsService
                .getSimilarProductsToExcel({
				filter : $('#SimilarProductsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					productName2Filter: $('#ProductName2FilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSimilarProductModalSaved', function () {
            getSimilarProducts();
        });

		$('#GetSimilarProductsButton').click(function (e) {
            e.preventDefault();
            getSimilarProducts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getSimilarProducts();
		  }
		});
    });
})();