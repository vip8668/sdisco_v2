(function () {
    $(function () {

        var _$productDetailsTable = $('#ProductDetailsTable');
        var _productDetailsService = abp.services.app.productDetails;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ProductDetails.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ProductDetails.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ProductDetails.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetails/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductDetails/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductDetailModal'
        });

		 var _viewProductDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetails/ViewproductDetailModal',
            modalClass: 'ViewProductDetailModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productDetailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productDetailsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductDetailsTableFilter').val(),
					titleFilter: $('#TitleFilterId').val(),
					minOrderFilter: $('#MinOrderFilterId').val(),
					maxOrderFilter: $('#MaxOrderFilterId').val(),
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
                                    _viewProductDetailModal.open({ id: data.record.productDetail.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productDetail.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductDetail(data.record.productDetail);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productDetail.title",
						 name: "title"   
					},
					{
						targets: 2,
						 data: "productDetail.order",
						 name: "order"   
					},
					{
						targets: 3,
						 data: "productDetail.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "productDetail.thumbImage",
						 name: "thumbImage"   
					},
					{
						targets: 5,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getProductDetails() {
            dataTable.ajax.reload();
        }

        function deleteProductDetail(productDetail) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productDetailsService.delete({
                            id: productDetail.id
                        }).done(function () {
                            getProductDetails(true);
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

        $('#CreateNewProductDetailButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productDetailsService
                .getProductDetailsToExcel({
				filter : $('#ProductDetailsTableFilter').val(),
					titleFilter: $('#TitleFilterId').val(),
					minOrderFilter: $('#MinOrderFilterId').val(),
					maxOrderFilter: $('#MaxOrderFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductDetailModalSaved', function () {
            getProductDetails();
        });

		$('#GetProductDetailsButton').click(function (e) {
            e.preventDefault();
            getProductDetails();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductDetails();
		  }
		});
    });
})();