(function () {
    $(function () {

        var _$helpCategoriesTable = $('#HelpCategoriesTable');
        var _helpCategoriesService = abp.services.app.helpCategories;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.HelpCategories.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.HelpCategories.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.HelpCategories.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/HelpCategories/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/HelpCategories/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHelpCategoryModal'
        });

		 var _viewHelpCategoryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/HelpCategories/ViewhelpCategoryModal',
            modalClass: 'ViewHelpCategoryModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$helpCategoriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _helpCategoriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HelpCategoriesTableFilter').val(),
					categoryNameFilter: $('#CategoryNameFilterId').val(),
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
                                    _viewHelpCategoryModal.open({ id: data.record.helpCategory.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.helpCategory.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHelpCategory(data.record.helpCategory);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "helpCategory.categoryName",
						 name: "categoryName"   
					},
					{
						targets: 2,
						 data: "helpCategory.type",
						 name: "type"   ,
						render: function (type) {
							return app.localize('Enum_HelpTypeEnum_' + type);
						}
			
					}
            ]
        });

        function getHelpCategories() {
            dataTable.ajax.reload();
        }

        function deleteHelpCategory(helpCategory) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _helpCategoriesService.delete({
                            id: helpCategory.id
                        }).done(function () {
                            getHelpCategories(true);
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

        $('#CreateNewHelpCategoryButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _helpCategoriesService
                .getHelpCategoriesToExcel({
				filter : $('#HelpCategoriesTableFilter').val(),
					categoryNameFilter: $('#CategoryNameFilterId').val(),
					typeFilter: $('#TypeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHelpCategoryModalSaved', function () {
            getHelpCategories();
        });

		$('#GetHelpCategoriesButton').click(function (e) {
            e.preventDefault();
            getHelpCategories();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHelpCategories();
		  }
		});
    });
})();