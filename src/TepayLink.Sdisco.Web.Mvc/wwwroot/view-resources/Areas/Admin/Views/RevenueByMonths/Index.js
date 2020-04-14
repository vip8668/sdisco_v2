(function () {
    $(function () {

        var _$revenueByMonthsTable = $('#RevenueByMonthsTable');
        var _revenueByMonthsService = abp.services.app.revenueByMonths;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.RevenueByMonths.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.RevenueByMonths.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.RevenueByMonths.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RevenueByMonths/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/RevenueByMonths/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRevenueByMonthModal'
        });

		 var _viewRevenueByMonthModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RevenueByMonths/ViewrevenueByMonthModal',
            modalClass: 'ViewRevenueByMonthModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$revenueByMonthsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _revenueByMonthsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RevenueByMonthsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					minRevenueFilter: $('#MinRevenueFilterId').val(),
					maxRevenueFilter: $('#MaxRevenueFilterId').val(),
					minDateFilter:  getDateFilter($('#MinDateFilterId')),
					maxDateFilter:  getDateFilter($('#MaxDateFilterId'))
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
                                    _viewRevenueByMonthModal.open({ id: data.record.revenueByMonth.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.revenueByMonth.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRevenueByMonth(data.record.revenueByMonth);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "revenueByMonth.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "revenueByMonth.revenue",
						 name: "revenue"   
					},
					{
						targets: 3,
						 data: "revenueByMonth.date",
						 name: "date" ,
					render: function (date) {
						if (date) {
							return moment(date).format('L');
						}
						return "";
					}
			  
					}
            ]
        });

        function getRevenueByMonths() {
            dataTable.ajax.reload();
        }

        function deleteRevenueByMonth(revenueByMonth) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _revenueByMonthsService.delete({
                            id: revenueByMonth.id
                        }).done(function () {
                            getRevenueByMonths(true);
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

        $('#CreateNewRevenueByMonthButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _revenueByMonthsService
                .getRevenueByMonthsToExcel({
				filter : $('#RevenueByMonthsTableFilter').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					minRevenueFilter: $('#MinRevenueFilterId').val(),
					maxRevenueFilter: $('#MaxRevenueFilterId').val(),
					minDateFilter:  getDateFilter($('#MinDateFilterId')),
					maxDateFilter:  getDateFilter($('#MaxDateFilterId'))
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRevenueByMonthModalSaved', function () {
            getRevenueByMonths();
        });

		$('#GetRevenueByMonthsButton').click(function (e) {
            e.preventDefault();
            getRevenueByMonths();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRevenueByMonths();
		  }
		});
    });
})();