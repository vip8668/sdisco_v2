(function () {
    $(function () {

        var _$bookingsTable = $('#BookingsTable');
        var _bookingsService = abp.services.app.bookings;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Bookings.Create'),
            edit: abp.auth.hasPermission('Pages.Bookings.Edit'),
            'delete': abp.auth.hasPermission('Pages.Bookings.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Bookings/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Bookings/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBookingModal'
        });

		 var _viewBookingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Bookings/ViewbookingModal',
            modalClass: 'ViewBookingModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$bookingsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bookingsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BookingsTableFilter').val(),
					statusFilter: $('#StatusFilterId').val(),
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
                                    _viewBookingModal.open({ id: data.record.booking.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.booking.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBooking(data.record.booking);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "booking.startDate",
						 name: "startDate" ,
					render: function (startDate) {
						if (startDate) {
							return moment(startDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 2,
						 data: "booking.endDate",
						 name: "endDate" ,
					render: function (endDate) {
						if (endDate) {
							return moment(endDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 3,
						 data: "booking.tripLength",
						 name: "tripLength"   
					},
					{
						targets: 4,
						 data: "booking.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_BookingStatusEnum_' + status);
						}
			
					},
					{
						targets: 5,
						 data: "booking.quantity",
						 name: "quantity"   
					},
					{
						targets: 6,
						 data: "booking.amount",
						 name: "amount"   
					},
					{
						targets: 7,
						 data: "booking.fee",
						 name: "fee"   
					},
					{
						targets: 8,
						 data: "booking.note",
						 name: "note"   
					},
					{
						targets: 9,
						 data: "booking.guestInfo",
						 name: "guestInfo"   
					},
					{
						targets: 10,
						 data: "booking.couponCode",
						 name: "couponCode"   
					},
					{
						targets: 11,
						 data: "booking.bonusAmount",
						 name: "bonusAmount"   
					},
					{
						targets: 12,
						 data: "booking.contact",
						 name: "contact"   
					},
					{
						targets: 13,
						 data: "booking.couponId",
						 name: "couponId"   
					},
					{
						targets: 14,
						 data: "booking.totalAmount",
						 name: "totalAmount"   
					},
					{
						targets: 15,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getBookings() {
            dataTable.ajax.reload();
        }

        function deleteBooking(booking) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bookingsService.delete({
                            id: booking.id
                        }).done(function () {
                            getBookings(true);
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

        $('#CreateNewBookingButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _bookingsService
                .getBookingsToExcel({
				filter : $('#BookingsTableFilter').val(),
					statusFilter: $('#StatusFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBookingModalSaved', function () {
            getBookings();
        });

		$('#GetBookingsButton').click(function (e) {
            e.preventDefault();
            getBookings();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBookings();
		  }
		});
    });
})();