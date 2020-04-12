(function () {
    $(function () {

        var _$bookingDetailsTable = $('#BookingDetailsTable');
        var _bookingDetailsService = abp.services.app.bookingDetails;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.BookingDetails.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.BookingDetails.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.BookingDetails.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingDetails/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BookingDetails/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBookingDetailModal'
        });

		 var _viewBookingDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingDetails/ViewbookingDetailModal',
            modalClass: 'ViewBookingDetailModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$bookingDetailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bookingDetailsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BookingDetailsTableFilter').val(),
					minRefundAmountFilter: $('#MinRefundAmountFilterId').val(),
					maxRefundAmountFilter: $('#MaxRefundAmountFilterId').val(),
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
                                    _viewBookingDetailModal.open({ id: data.record.bookingDetail.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.bookingDetail.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBookingDetail(data.record.bookingDetail);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bookingDetail.bookingId",
						 name: "bookingId"   
					},
					{
						targets: 2,
						 data: "bookingDetail.startDate",
						 name: "startDate" ,
					render: function (startDate) {
						if (startDate) {
							return moment(startDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 3,
						 data: "bookingDetail.endDate",
						 name: "endDate" ,
					render: function (endDate) {
						if (endDate) {
							return moment(endDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 4,
						 data: "bookingDetail.tripLength",
						 name: "tripLength" ,
					render: function (tripLength) {
						if (tripLength) {
							return moment(tripLength).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "bookingDetail.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_BookingStatusEnum_' + status);
						}
			
					},
					{
						targets: 6,
						 data: "bookingDetail.productScheduleId",
						 name: "productScheduleId"   
					},
					{
						targets: 7,
						 data: "bookingDetail.quantity",
						 name: "quantity"   
					},
					{
						targets: 8,
						 data: "bookingDetail.amount",
						 name: "amount"   
					},
					{
						targets: 9,
						 data: "bookingDetail.fee",
						 name: "fee"   
					},
					{
						targets: 10,
						 data: "bookingDetail.hostPaymentStatus",
						 name: "hostPaymentStatus"   
					},
					{
						targets: 11,
						 data: "bookingDetail.hostUserId",
						 name: "hostUserId"   
					},
					{
						targets: 12,
						 data: "bookingDetail.bookingUserId",
						 name: "bookingUserId"   
					},
					{
						targets: 13,
						 data: "bookingDetail.isDone",
						 name: "isDone"  ,
						render: function (isDone) {
							if (isDone) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 14,
						 data: "bookingDetail.affiliateUserId",
						 name: "affiliateUserId"   
					},
					{
						targets: 15,
						 data: "bookingDetail.roomId",
						 name: "roomId"   
					},
					{
						targets: 16,
						 data: "bookingDetail.note",
						 name: "note"   
					},
					{
						targets: 17,
						 data: "bookingDetail.cancelDate",
						 name: "cancelDate" ,
					render: function (cancelDate) {
						if (cancelDate) {
							return moment(cancelDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 18,
						 data: "bookingDetail.refundAmount",
						 name: "refundAmount"   
					},
					{
						targets: 19,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getBookingDetails() {
            dataTable.ajax.reload();
        }

        function deleteBookingDetail(bookingDetail) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bookingDetailsService.delete({
                            id: bookingDetail.id
                        }).done(function () {
                            getBookingDetails(true);
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

        $('#CreateNewBookingDetailButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _bookingDetailsService
                .getBookingDetailsToExcel({
				filter : $('#BookingDetailsTableFilter').val(),
					minRefundAmountFilter: $('#MinRefundAmountFilterId').val(),
					maxRefundAmountFilter: $('#MaxRefundAmountFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBookingDetailModalSaved', function () {
            getBookingDetails();
        });

		$('#GetBookingDetailsButton').click(function (e) {
            e.preventDefault();
            getBookingDetails();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBookingDetails();
		  }
		});
    });
})();