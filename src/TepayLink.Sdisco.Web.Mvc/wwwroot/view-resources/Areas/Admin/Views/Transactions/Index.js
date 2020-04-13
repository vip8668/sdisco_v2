(function () {
    $(function () {

        var _$transactionsTable = $('#TransactionsTable');
        var _transactionsService = abp.services.app.transactions;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Transactions.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Transactions.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Transactions.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Transactions/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Transactions/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTransactionModal'
        });

		 var _viewTransactionModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Transactions/ViewtransactionModal',
            modalClass: 'ViewTransactionModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$transactionsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _transactionsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#TransactionsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					minSideFilter: $('#MinSideFilterId').val(),
					maxSideFilter: $('#MaxSideFilterId').val(),
					transTypeFilter: $('#TransTypeFilterId').val(),
					walletTypeFilter: $('#WalletTypeFilterId').val(),
					minBookingDetailIdFilter: $('#MinBookingDetailIdFilterId').val(),
					maxBookingDetailIdFilter: $('#MaxBookingDetailIdFilterId').val(),
					minRefIdFilter: $('#MinRefIdFilterId').val(),
					maxRefIdFilter: $('#MaxRefIdFilterId').val(),
					descritionFilter: $('#DescritionFilterId').val()
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
                                    _viewTransactionModal.open({ id: data.record.transaction.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.transaction.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteTransaction(data.record.transaction);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "transaction.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "transaction.amount",
						 name: "amount"   
					},
					{
						targets: 3,
						 data: "transaction.side",
						 name: "side"   
					},
					{
						targets: 4,
						 data: "transaction.transType",
						 name: "transType"   ,
						render: function (transType) {
							return app.localize('Enum_TransactionType_' + transType);
						}
			
					},
					{
						targets: 5,
						 data: "transaction.walletType",
						 name: "walletType"   ,
						render: function (walletType) {
							return app.localize('Enum_WalletTypeEnum_' + walletType);
						}
			
					},
					{
						targets: 6,
						 data: "transaction.bookingDetailId",
						 name: "bookingDetailId"   
					},
					{
						targets: 7,
						 data: "transaction.refId",
						 name: "refId"   
					},
					{
						targets: 8,
						 data: "transaction.descrition",
						 name: "descrition"   
					}
            ]
        });

        function getTransactions() {
            dataTable.ajax.reload();
        }

        function deleteTransaction(transaction) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _transactionsService.delete({
                            id: transaction.id
                        }).done(function () {
                            getTransactions(true);
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

        $('#CreateNewTransactionButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _transactionsService
                .getTransactionsToExcel({
				filter : $('#TransactionsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					minSideFilter: $('#MinSideFilterId').val(),
					maxSideFilter: $('#MaxSideFilterId').val(),
					transTypeFilter: $('#TransTypeFilterId').val(),
					walletTypeFilter: $('#WalletTypeFilterId').val(),
					minBookingDetailIdFilter: $('#MinBookingDetailIdFilterId').val(),
					maxBookingDetailIdFilter: $('#MaxBookingDetailIdFilterId').val(),
					minRefIdFilter: $('#MinRefIdFilterId').val(),
					maxRefIdFilter: $('#MaxRefIdFilterId').val(),
					descritionFilter: $('#DescritionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTransactionModalSaved', function () {
            getTransactions();
        });

		$('#GetTransactionsButton').click(function (e) {
            e.preventDefault();
            getTransactions();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getTransactions();
		  }
		});
    });
})();