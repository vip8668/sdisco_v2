(function () {
    $(function () {

        var _$productDetailCombosTable = $('#ProductDetailCombosTable');
        var _productDetailCombosService = abp.services.app.productDetailCombos;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ProductDetailCombos.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ProductDetailCombos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ProductDetailCombos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetailCombos/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductDetailCombos/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductDetailComboModal'
        });

		 var _viewProductDetailComboModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetailCombos/ViewproductDetailComboModal',
            modalClass: 'ViewProductDetailComboModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productDetailCombosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productDetailCombosService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductDetailCombosTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					productDetailTitleFilter: $('#ProductDetailTitleFilterId').val(),
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
                                    _viewProductDetailComboModal.open({ id: data.record.productDetailCombo.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productDetailCombo.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductDetailCombo(data.record.productDetailCombo);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productDetailCombo.roomId",
						 name: "roomId"   
					},
					{
						targets: 2,
						 data: "productDetailCombo.description",
						 name: "description"   
					},
					{
						targets: 3,
						 data: "productName" ,
						 name: "productFk.name" 
					},
					{
						targets: 4,
						 data: "productDetailTitle" ,
						 name: "productDetailFk.title" 
					},
					{
						targets: 5,
						 data: "productName2" ,
						 name: "itemFk.name" 
					}
            ]
        });

        function getProductDetailCombos() {
            dataTable.ajax.reload();
        }

        function deleteProductDetailCombo(productDetailCombo) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productDetailCombosService.delete({
                            id: productDetailCombo.id
                        }).done(function () {
                            getProductDetailCombos(true);
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

        $('#CreateNewProductDetailComboButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productDetailCombosService
                .getProductDetailCombosToExcel({
				filter : $('#ProductDetailCombosTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val(),
					productDetailTitleFilter: $('#ProductDetailTitleFilterId').val(),
					productName2Filter: $('#ProductName2FilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductDetailComboModalSaved', function () {
            getProductDetailCombos();
        });

		$('#GetProductDetailCombosButton').click(function (e) {
            e.preventDefault();
            getProductDetailCombos();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductDetailCombos();
		  }
		});
    });
})();