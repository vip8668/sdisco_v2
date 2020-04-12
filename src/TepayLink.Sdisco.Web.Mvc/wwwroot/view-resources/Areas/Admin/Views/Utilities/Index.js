(function () {
    $(function () {

        var _$utilitiesTable = $('#UtilitiesTable');
        var _utilitiesService = abp.services.app.utilities;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Utilities.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Utilities.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Utilities.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Utilities/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Utilities/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUtilityModal'
        });

		 var _viewUtilityModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Utilities/ViewutilityModal',
            modalClass: 'ViewUtilityModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$utilitiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _utilitiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#UtilitiesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					iconFilter: $('#IconFilterId').val()
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
                                    _viewUtilityModal.open({ id: data.record.utility.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.utility.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUtility(data.record.utility);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "utility.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "utility.icon",
						 name: "icon"   
					}
            ]
        });

        function getUtilities() {
            dataTable.ajax.reload();
        }

        function deleteUtility(utility) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _utilitiesService.delete({
                            id: utility.id
                        }).done(function () {
                            getUtilities(true);
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

        $('#CreateNewUtilityButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _utilitiesService
                .getUtilitiesToExcel({
				filter : $('#UtilitiesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					iconFilter: $('#IconFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditUtilityModalSaved', function () {
            getUtilities();
        });

		$('#GetUtilitiesButton').click(function (e) {
            e.preventDefault();
            getUtilities();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getUtilities();
		  }
		});
    });
})();