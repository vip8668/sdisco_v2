(function () {
    $(function () {

        var _$bankBranchsTable = $('#BankBranchsTable');
        var _bankBranchsService = abp.services.app.bankBranchs;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.BankBranchs.Create'),
            edit: abp.auth.hasPermission('Pages.BankBranchs.Edit'),
            'delete': abp.auth.hasPermission('Pages.BankBranchs.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankBranchs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BankBranchs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBankBranchModal'
        });

		 var _viewBankBranchModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankBranchs/ViewbankBranchModal',
            modalClass: 'ViewBankBranchModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$bankBranchsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _bankBranchsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BankBranchsTableFilter').val(),
					bankBankNameFilter: $('#BankBankNameFilterId').val()
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
                                    _viewBankBranchModal.open({ id: data.record.bankBranch.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.bankBranch.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBankBranch(data.record.bankBranch);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "bankBranch.branchName",
						 name: "branchName"   
					},
					{
						targets: 2,
						 data: "bankBranch.address",
						 name: "address"   
					},
					{
						targets: 3,
						 data: "bankBranch.order",
						 name: "order"   
					},
					{
						targets: 4,
						 data: "bankBankName" ,
						 name: "bankFk.bankName" 
					}
            ]
        });

        function getBankBranchs() {
            dataTable.ajax.reload();
        }

        function deleteBankBranch(bankBranch) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bankBranchsService.delete({
                            id: bankBranch.id
                        }).done(function () {
                            getBankBranchs(true);
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

        $('#CreateNewBankBranchButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _bankBranchsService
                .getBankBranchsToExcel({
				filter : $('#BankBranchsTableFilter').val(),
					bankBankNameFilter: $('#BankBankNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBankBranchModalSaved', function () {
            getBankBranchs();
        });

		$('#GetBankBranchsButton').click(function (e) {
            e.preventDefault();
            getBankBranchs();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBankBranchs();
		  }
		});
    });
})();