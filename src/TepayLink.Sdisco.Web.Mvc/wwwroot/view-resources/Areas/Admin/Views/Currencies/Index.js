(function () {
    $(function () {

        var _$currenciesTable = $('#CurrenciesTable');
        var _currenciesService = abp.services.app.currencies;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Currencies.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Currencies.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Currencies.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Currencies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Currencies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCurrencyModal'
        });

		 var _viewCurrencyModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Currencies/ViewcurrencyModal',
            modalClass: 'ViewCurrencyModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$currenciesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _currenciesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CurrenciesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					displayNameFilter: $('#DisplayNameFilterId').val(),
					iconFilter: $('#IconFilterId').val(),
					currencySignFilter: $('#CurrencySignFilterId').val(),
					isDisabledFilter: $('#IsDisabledFilterId').val()
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
                                    _viewCurrencyModal.open({ id: data.record.currency.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.currency.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCurrency(data.record.currency);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "currency.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "currency.displayName",
						 name: "displayName"   
					},
					{
						targets: 3,
						 data: "currency.icon",
						 name: "icon"   
					},
					{
						targets: 4,
						 data: "currency.currencySign",
						 name: "currencySign"   
					},
					{
						targets: 5,
						 data: "currency.isDisabled",
						 name: "isDisabled"  ,
						render: function (isDisabled) {
							if (isDisabled) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getCurrencies() {
            dataTable.ajax.reload();
        }

        function deleteCurrency(currency) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _currenciesService.delete({
                            id: currency.id
                        }).done(function () {
                            getCurrencies(true);
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

        $('#CreateNewCurrencyButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _currenciesService
                .getCurrenciesToExcel({
				filter : $('#CurrenciesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					displayNameFilter: $('#DisplayNameFilterId').val(),
					iconFilter: $('#IconFilterId').val(),
					currencySignFilter: $('#CurrencySignFilterId').val(),
					isDisabledFilter: $('#IsDisabledFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCurrencyModalSaved', function () {
            getCurrencies();
        });

		$('#GetCurrenciesButton').click(function (e) {
            e.preventDefault();
            getCurrencies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCurrencies();
		  }
		});
    });
})();