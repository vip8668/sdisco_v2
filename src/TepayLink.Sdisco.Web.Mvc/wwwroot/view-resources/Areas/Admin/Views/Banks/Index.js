(function () {
    $(function () {

        var _$banksTable = $('#BanksTable');
        var _banksService = abp.services.app.banks;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Banks.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Banks.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Banks.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Banks/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Banks/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBankModal'
        });

		 var _viewBankModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Banks/ViewbankModal',
            modalClass: 'ViewBankModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$banksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _banksService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BanksTableFilter').val(),
					bankNameFilter: $('#BankNameFilterId').val(),
					bankCodeFilter: $('#BankCodeFilterId').val(),
					displayNameFilter: $('#DisplayNameFilterId').val(),
					typeFilter: $('#TypeFilterId').val()
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
                                    _viewBankModal.open({ id: data.record.bank.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.bank.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBank(data.record.bank);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bank.bankName",
						 name: "bankName"   
					},
					{
						targets: 2,
						 data: "bank.bankCode",
						 name: "bankCode"   
					},
					{
						targets: 3,
						 data: "bank.displayName",
						 name: "displayName"   
					},
					{
						targets: 4,
						 data: "bank.type",
						 name: "type"   ,
						render: function (type) {
							return app.localize('Enum_BankTypeEnum_' + type);
						}
			
					},
					{
						targets: 5,
						 data: "bank.order",
						 name: "order"   
					},
					{
						targets: 6,
						 data: "bank.logo",
						 name: "logo"   
					},
					{
						targets: 7,
						 data: "bank.cardImage",
						 name: "cardImage"   
					},
					{
						targets: 8,
						 data: "bank.description",
						 name: "description"   
					}
            ]
        });

        function getBanks() {
            dataTable.ajax.reload();
        }

        function deleteBank(bank) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _banksService.delete({
                            id: bank.id
                        }).done(function () {
                            getBanks(true);
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

        $('#CreateNewBankButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _banksService
                .getBanksToExcel({
				filter : $('#BanksTableFilter').val(),
					bankNameFilter: $('#BankNameFilterId').val(),
					bankCodeFilter: $('#BankCodeFilterId').val(),
					displayNameFilter: $('#DisplayNameFilterId').val(),
					typeFilter: $('#TypeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBankModalSaved', function () {
            getBanks();
        });

		$('#GetBanksButton').click(function (e) {
            e.preventDefault();
            getBanks();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBanks();
		  }
		});
    });
})();