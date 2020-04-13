(function () {
    $(function () {

        var _$productReviewDetailsTable = $('#ProductReviewDetailsTable');
        var _productReviewDetailsService = abp.services.app.productReviewDetails;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ProductReviewDetails.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ProductReviewDetails.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ProductReviewDetails.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductReviewDetails/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductReviewDetails/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditProductReviewDetailModal'
        });

		 var _viewProductReviewDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductReviewDetails/ViewproductReviewDetailModal',
            modalClass: 'ViewProductReviewDetailModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$productReviewDetailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _productReviewDetailsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ProductReviewDetailsTableFilter').val(),
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
                                    _viewProductReviewDetailModal.open({ id: data.record.productReviewDetail.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.productReviewDetail.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteProductReviewDetail(data.record.productReviewDetail);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "productReviewDetail.ratingAvg",
						 name: "ratingAvg"   
					},
					{
						targets: 2,
						 data: "productReviewDetail.intineraty",
						 name: "intineraty"   
					},
					{
						targets: 3,
						 data: "productReviewDetail.service",
						 name: "service"   
					},
					{
						targets: 4,
						 data: "productReviewDetail.transport",
						 name: "transport"   
					},
					{
						targets: 5,
						 data: "productReviewDetail.guideTour",
						 name: "guideTour"   
					},
					{
						targets: 6,
						 data: "productReviewDetail.food",
						 name: "food"   
					},
					{
						targets: 7,
						 data: "productReviewDetail.title",
						 name: "title"   
					},
					{
						targets: 8,
						 data: "productReviewDetail.comment",
						 name: "comment"   
					},
					{
						targets: 9,
						 data: "productReviewDetail.bookingId",
						 name: "bookingId"   
					},
					{
						targets: 10,
						 data: "productReviewDetail.read",
						 name: "read"  ,
						render: function (read) {
							if (read) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 11,
						 data: "productReviewDetail.replyComment",
						 name: "replyComment"   
					},
					{
						targets: 12,
						 data: "productReviewDetail.replyId",
						 name: "replyId"   
					},
					{
						targets: 13,
						 data: "productReviewDetail.avatar",
						 name: "avatar"   
					},
					{
						targets: 14,
						 data: "productReviewDetail.reviewer",
						 name: "reviewer"   
					},
					{
						targets: 15,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getProductReviewDetails() {
            dataTable.ajax.reload();
        }

        function deleteProductReviewDetail(productReviewDetail) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _productReviewDetailsService.delete({
                            id: productReviewDetail.id
                        }).done(function () {
                            getProductReviewDetails(true);
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

        $('#CreateNewProductReviewDetailButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _productReviewDetailsService
                .getProductReviewDetailsToExcel({
				filter : $('#ProductReviewDetailsTableFilter').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditProductReviewDetailModalSaved', function () {
            getProductReviewDetails();
        });

		$('#GetProductReviewDetailsButton').click(function (e) {
            e.preventDefault();
            getProductReviewDetails();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getProductReviewDetails();
		  }
		});
    });
})();