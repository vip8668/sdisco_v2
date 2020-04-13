(function () {
    $(function () {

        var _$shareTransactionsTable = $('#ShareTransactionsTable');
        var _shareTransactionsService = abp.services.app.shareTransactions;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.ShareTransactions.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.ShareTransactions.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.ShareTransactions.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ShareTransactions/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ShareTransactions/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditShareTransactionModal'
        });

		 var _viewShareTransactionModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ShareTransactions/ViewshareTransactionModal',
            modalClass: 'ViewShareTransactionModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$shareTransactionsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _shareTransactionsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ShareTransactionsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					typeFilter: $('#TypeFilterId').val(),
					iPFilter: $('#IPFilterId').val(),
					minPointFilter: $('#MinPointFilterId').val(),
					maxPointFilter: $('#MaxPointFilterId').val(),
					minProductIdFilter: $('#MinProductIdFilterId').val(),
					maxProductIdFilter: $('#MaxProductIdFilterId').val()
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
                                    _viewShareTransactionModal.open({ id: data.record.shareTransaction.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.shareTransaction.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteShareTransaction(data.record.shareTransaction);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "shareTransaction.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "shareTransaction.type",
						 name: "type"   ,
						render: function (type) {
							return app.localize('Enum_RevenueTypeEnum_' + type);
						}
			
					},
					{
						targets: 3,
						 data: "shareTransaction.ip",
						 name: "ip"   
					},
					{
						targets: 4,
						 data: "shareTransaction.point",
						 name: "point"   
					},
					{
						targets: 5,
						 data: "shareTransaction.productId",
						 name: "productId"   
					}
            ]
        });

        function getShareTransactions() {
            dataTable.ajax.reload();
        }

        function deleteShareTransaction(shareTransaction) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _shareTransactionsService.delete({
                            id: shareTransaction.id
                        }).done(function () {
                            getShareTransactions(true);
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

        $('#CreateNewShareTransactionButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _shareTransactionsService
                .getShareTransactionsToExcel({
				filter : $('#ShareTransactionsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					typeFilter: $('#TypeFilterId').val(),
					iPFilter: $('#IPFilterId').val(),
					minPointFilter: $('#MinPointFilterId').val(),
					maxPointFilter: $('#MaxPointFilterId').val(),
					minProductIdFilter: $('#MinProductIdFilterId').val(),
					maxProductIdFilter: $('#MaxProductIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditShareTransactionModalSaved', function () {
            getShareTransactions();
        });

		$('#GetShareTransactionsButton').click(function (e) {
            e.preventDefault();
            getShareTransactions();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getShareTransactions();
		  }
		});
    });
})();