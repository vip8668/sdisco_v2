(function () {
    $(function () {

        var _$productsTable = $('#ProductsTable');
        var _productsService = abp.services.app.products;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Products.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Products.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Products.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Products/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Products/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductModal'
        });

		 var _viewProductModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Products/ViewproductModal',
            modalClass: 'ViewProductModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					typeFilter: $('#TypeFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					includeTourGuideFilter: $('#IncludeTourGuideFilterId').val(),
					allowRetailFilter: $('#AllowRetailFilterId').val(),
					minPriceFilter: $('#MinPriceFilterId').val(),
					maxPriceFilter: $('#MaxPriceFilterId').val(),
					isTrendingFilter: $('#IsTrendingFilterId').val(),
					categoryNameFilter: $('#CategoryNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					placeNameFilter: $('#PlaceNameFilterId').val(),
					applicationLanguageNameFilter: $('#ApplicationLanguageNameFilterId').val()
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
                                    _viewProductModal.open({ id: data.record.product.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.product.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProduct(data.record.product);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "product.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "product.type",
						 name: "type"   ,
						render: function (type) {
							return app.localize('Enum_ProductTypeEnum_' + type);
						}
			
					},
					{
						targets: 3,
						 data: "product.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_ProductStatusEnum_' + status);
						}
			
					},
					{
						targets: 4,
						 data: "product.description",
						 name: "description"   
					},
					{
						targets: 5,
						 data: "product.policies",
						 name: "policies"   
					},
					{
						targets: 6,
						 data: "product.duration",
						 name: "duration"   
					},
					{
						targets: 7,
						 data: "product.startTime",
						 name: "startTime"   
					},
					{
						targets: 8,
						 data: "product.includeTourGuide",
						 name: "includeTourGuide"  ,
						render: function (includeTourGuide) {
							if (includeTourGuide) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 9,
						 data: "product.allowRetail",
						 name: "allowRetail"  ,
						render: function (allowRetail) {
							if (allowRetail) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 10,
						 data: "product.totalSlot",
						 name: "totalSlot"   
					},
					{
						targets: 11,
						 data: "product.price",
						 name: "price"   
					},
					{
						targets: 12,
						 data: "product.instantBook",
						 name: "instantBook"  ,
						render: function (instantBook) {
							if (instantBook) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 13,
						 data: "product.tripLengh",
						 name: "tripLengh"   
					},
					{
						targets: 14,
						 data: "product.isHotDeal",
						 name: "isHotDeal"  ,
						render: function (isHotDeal) {
							if (isHotDeal) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 15,
						 data: "product.isBestSeller",
						 name: "isBestSeller"  ,
						render: function (isBestSeller) {
							if (isBestSeller) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 16,
						 data: "product.isTrending",
						 name: "isTrending"  ,
						render: function (isTrending) {
							if (isTrending) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 17,
						 data: "product.isTop",
						 name: "isTop"  ,
						render: function (isTop) {
							if (isTop) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 18,
						 data: "categoryName" ,
						 name: "categoryFk.name" 
					},
					{
						targets: 19,
						 data: "userName" ,
						 name: "hostUserFk.name" 
					},
					{
						targets: 20,
						 data: "placeName" ,
						 name: "placeFk.name" 
					},
					{
						targets: 21,
						 data: "applicationLanguageName" ,
						 name: "languageFk.name" 
					}
            ]
        });

        function getProducts() {
            dataTable.ajax.reload();
        }

        function deleteProduct(product) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productsService.delete({
                            id: product.id
                        }).done(function () {
                            getProducts(true);
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

        $('#CreateNewProductButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productsService
                .getProductsToExcel({
				filter : $('#ProductsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					typeFilter: $('#TypeFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					includeTourGuideFilter: $('#IncludeTourGuideFilterId').val(),
					allowRetailFilter: $('#AllowRetailFilterId').val(),
					minPriceFilter: $('#MinPriceFilterId').val(),
					maxPriceFilter: $('#MaxPriceFilterId').val(),
					isTrendingFilter: $('#IsTrendingFilterId').val(),
					categoryNameFilter: $('#CategoryNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					placeNameFilter: $('#PlaceNameFilterId').val(),
					applicationLanguageNameFilter: $('#ApplicationLanguageNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductModalSaved', function () {
            getProducts();
        });

		$('#GetProductsButton').click(function (e) {
            e.preventDefault();
            getProducts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProducts();
		  }
		});
    });
})();