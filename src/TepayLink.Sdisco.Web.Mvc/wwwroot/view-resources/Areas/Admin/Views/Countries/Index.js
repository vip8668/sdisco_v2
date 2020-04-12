(function () {
    $(function () {

        var _$countriesTable = $('#CountriesTable');
        var _countriesService = abp.services.app.countries;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Countries.Create'),
            edit: abp.auth.hasPermission('Pages.Countries.Edit'),
            'delete': abp.auth.hasPermission('Pages.Countries.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Countries/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Countries/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCountryModal'
        });

		 var _viewCountryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Countries/ViewcountryModal',
            modalClass: 'ViewCountryModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$countriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _countriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CountriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					displayNameFilter: $('#DisplayNameFilterId').val(),
					iconFilter: $('#IconFilterId').val(),
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
                                    _viewCountryModal.open({ id: data.record.country.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.country.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCountry(data.record.country);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "country.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "country.displayName",
						 name: "displayName"   
					},
					{
						targets: 3,
						 data: "country.icon",
						 name: "icon"   
					},
					{
						targets: 4,
						 data: "country.isDisabled",
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

        function getCountries() {
            dataTable.ajax.reload();
        }

        function deleteCountry(country) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _countriesService.delete({
                            id: country.id
                        }).done(function () {
                            getCountries(true);
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

        $('#CreateNewCountryButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _countriesService
                .getCountriesToExcel({
				filter : $('#CountriesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					displayNameFilter: $('#DisplayNameFilterId').val(),
					iconFilter: $('#IconFilterId').val(),
					isDisabledFilter: $('#IsDisabledFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCountryModalSaved', function () {
            getCountries();
        });

		$('#GetCountriesButton').click(function (e) {
            e.preventDefault();
            getCountries();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCountries();
		  }
		});
    });
})();