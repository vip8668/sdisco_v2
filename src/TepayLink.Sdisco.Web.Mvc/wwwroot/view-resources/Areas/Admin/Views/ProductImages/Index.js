(function () {
    $(function () {

        var _$productImagesTable = $('#ProductImagesTable');
        var _productImagesService = abp.services.app.productImages;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ProductImages.Create'),
            edit: abp.auth.hasPermission('Pages.ProductImages.Edit'),
            'delete': abp.auth.hasPermission('Pages.ProductImages.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductImages/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductImages/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductImageModal'
        });

		 var _viewProductImageModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductImages/ViewproductImageModal',
            modalClass: 'ViewProductImageModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productImagesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productImagesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductImagesTableFilter').val(),
					imageTypeFilter: $('#ImageTypeFilterId').val(),
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
                                    _viewProductImageModal.open({ id: data.record.productImage.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productImage.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductImage(data.record.productImage);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productImage.url",
						 name: "url"   
					},
					{
						targets: 2,
						 data: "productImage.imageType",
						 name: "imageType"   ,
						render: function (imageType) {
							return app.localize('Enum_ImageType_' + imageType);
						}
			
					},
					{
						targets: 3,
						 data: "productImage.tag",
						 name: "tag"   
					},
					{
						targets: 4,
						 data: "productImage.title",
						 name: "title"   
					},
					{
						targets: 5,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getProductImages() {
            dataTable.ajax.reload();
        }

        function deleteProductImage(productImage) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productImagesService.delete({
                            id: productImage.id
                        }).done(function () {
                            getProductImages(true);
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

        $('#CreateNewProductImageButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productImagesService
                .getProductImagesToExcel({
				filter : $('#ProductImagesTableFilter').val(),
					imageTypeFilter: $('#ImageTypeFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductImageModalSaved', function () {
            getProductImages();
        });

		$('#GetProductImagesButton').click(function (e) {
            e.preventDefault();
            getProductImages();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductImages();
		  }
		});
    });
})();