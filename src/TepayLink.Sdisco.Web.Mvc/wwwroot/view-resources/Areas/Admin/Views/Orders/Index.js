(function () {
    $(function () {

        var _$ordersTable = $('#OrdersTable');
        var _ordersService = abp.services.app.orders;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Orders.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Orders.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Orders.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Orders/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Orders/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditOrderModal'
        });

		 var _viewOrderModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Orders/VieworderModal',
            modalClass: 'ViewOrderModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$ordersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _ordersService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#OrdersTableFilter').val(),
					issueDateFilter: $('#IssueDateFilterId').val(),
					nameOnCardFilter: $('#NameOnCardFilterId').val(),
					transactionIdFilter: $('#TransactionIdFilterId').val()
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
                                    _viewOrderModal.open({ id: data.record.order.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.order.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteOrder(data.record.order);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "order.orderCode",
						 name: "orderCode"   
					},
					{
						targets: 2,
						 data: "order.orderType",
						 name: "orderType"   ,
						render: function (orderType) {
							return app.localize('Enum_OrderTypeEnum_' + orderType);
						}
			
					},
					{
						targets: 3,
						 data: "order.amount",
						 name: "amount"   
					},
					{
						targets: 4,
						 data: "order.note",
						 name: "note"   
					},
					{
						targets: 5,
						 data: "order.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_OrderStatus_' + status);
						}
			
					},
					{
						targets: 6,
						 data: "order.orderRef",
						 name: "orderRef"   
					},
					{
						targets: 7,
						 data: "order.userId",
						 name: "userId"   
					},
					{
						targets: 8,
						 data: "order.bankCode",
						 name: "bankCode"   
					},
					{
						targets: 9,
						 data: "order.cardId",
						 name: "cardId"   
					},
					{
						targets: 10,
						 data: "order.cardNumber",
						 name: "cardNumber"   
					},
					{
						targets: 11,
						 data: "order.currency",
						 name: "currency"   
					},
					{
						targets: 12,
						 data: "order.issueDate",
						 name: "issueDate"   
					},
					{
						targets: 13,
						 data: "order.nameOnCard",
						 name: "nameOnCard"   
					},
					{
						targets: 14,
						 data: "order.transactionId",
						 name: "transactionId"   
					},
					{
						targets: 15,
						 data: "order.bookingId",
						 name: "bookingId"   
					}
            ]
        });

        function getOrders() {
            dataTable.ajax.reload();
        }

        function deleteOrder(order) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ordersService.delete({
                            id: order.id
                        }).done(function () {
                            getOrders(true);
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

        $('#CreateNewOrderButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _ordersService
                .getOrdersToExcel({
				filter : $('#OrdersTableFilter').val(),
					issueDateFilter: $('#IssueDateFilterId').val(),
					nameOnCardFilter: $('#NameOnCardFilterId').val(),
					transactionIdFilter: $('#TransactionIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditOrderModalSaved', function () {
            getOrders();
        });

		$('#GetOrdersButton').click(function (e) {
            e.preventDefault();
            getOrders();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getOrders();
		  }
		});
    });
})();