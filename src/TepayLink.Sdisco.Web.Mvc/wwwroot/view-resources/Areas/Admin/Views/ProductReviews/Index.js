(function () {
    $(function () {

        var _$productReviewsTable = $('#ProductReviewsTable');
        var _productReviewsService = abp.services.app.productReviews;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ProductReviews.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ProductReviews.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ProductReviews.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductReviews/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductReviews/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductReviewModal'
        });

		 var _viewProductReviewModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductReviews/ViewproductReviewModal',
            modalClass: 'ViewProductReviewModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productReviewsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productReviewsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductReviewsTableFilter').val(),
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
                                    _viewProductReviewModal.open({ id: data.record.productReview.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productReview.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductReview(data.record.productReview);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productReview.ratingAvg",
						 name: "ratingAvg"   
					},
					{
						targets: 2,
						 data: "productReview.reviewCount",
						 name: "reviewCount"   
					},
					{
						targets: 3,
						 data: "productReview.intineraty",
						 name: "intineraty"   
					},
					{
						targets: 4,
						 data: "productReview.service",
						 name: "service"   
					},
					{
						targets: 5,
						 data: "productReview.transport",
						 name: "transport"   
					},
					{
						targets: 6,
						 data: "productReview.guideTour",
						 name: "guideTour"   
					},
					{
						targets: 7,
						 data: "productReview.food",
						 name: "food"   
					},
					{
						targets: 8,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getProductReviews() {
            dataTable.ajax.reload();
        }

        function deleteProductReview(productReview) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productReviewsService.delete({
                            id: productReview.id
                        }).done(function () {
                            getProductReviews(true);
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

        $('#CreateNewProductReviewButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productReviewsService
                .getProductReviewsToExcel({
				filter : $('#ProductReviewsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductReviewModalSaved', function () {
            getProductReviews();
        });

		$('#GetProductReviewsButton').click(function (e) {
            e.preventDefault();
            getProductReviews();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductReviews();
		  }
		});
    });
})();