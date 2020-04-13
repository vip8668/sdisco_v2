(function () {
    $(function () {

        var _$detinationsTable = $('#DetinationsTable');
        var _detinationsService = abp.services.app.detinations;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Detinations.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Detinations.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Detinations.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Detinations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Detinations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDetinationModal'
        });

		 var _viewDetinationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Detinations/ViewdetinationModal',
            modalClass: 'ViewDetinationModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$detinationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _detinationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DetinationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					isTopFilter: $('#IsTopFilterId').val(),
					minBookingCountFilter: $('#MinBookingCountFilterId').val(),
					maxBookingCountFilter: $('#MaxBookingCountFilterId').val()
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
                                    _viewDetinationModal.open({ id: data.record.detination.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.detination.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDetination(data.record.detination);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "detination.image",
						 name: "image"   
					},
					{
						targets: 2,
						 data: "detination.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "detination.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_DetinationStatusEnum_' + status);
						}
			
					},
					{
						targets: 4,
						 data: "detination.isTop",
						 name: "isTop"  ,
						render: function (isTop) {
							if (isTop) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 5,
						 data: "detination.bookingCount",
						 name: "bookingCount"   
					}
            ]
        });

        function getDetinations() {
            dataTable.ajax.reload();
        }

        function deleteDetination(detination) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _detinationsService.delete({
                            id: detination.id
                        }).done(function () {
                            getDetinations(true);
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

        $('#CreateNewDetinationButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _detinationsService
                .getDetinationsToExcel({
				filter : $('#DetinationsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					isTopFilter: $('#IsTopFilterId').val(),
					minBookingCountFilter: $('#MinBookingCountFilterId').val(),
					maxBookingCountFilter: $('#MaxBookingCountFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDetinationModalSaved', function () {
            getDetinations();
        });

		$('#GetDetinationsButton').click(function (e) {
            e.preventDefault();
            getDetinations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDetinations();
		  }
		});
    });
})();