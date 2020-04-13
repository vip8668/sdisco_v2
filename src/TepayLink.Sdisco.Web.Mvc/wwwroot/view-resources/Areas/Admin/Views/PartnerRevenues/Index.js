(function () {
    $(function () {

        var _$partnerRevenuesTable = $('#PartnerRevenuesTable');
        var _partnerRevenuesService = abp.services.app.partnerRevenues;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.PartnerRevenues.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.PartnerRevenues.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.PartnerRevenues.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/PartnerRevenues/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/PartnerRevenues/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPartnerRevenueModal'
        });

		 var _viewPartnerRevenueModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/PartnerRevenues/ViewpartnerRevenueModal',
            modalClass: 'ViewPartnerRevenueModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$partnerRevenuesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _partnerRevenuesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PartnerRevenuesTableFilter').val(),
					minUseridFilter: $('#MinUseridFilterId').val(),
					maxUseridFilter: $('#MaxUseridFilterId').val(),
					revenueTypeFilter: $('#RevenueTypeFilterId').val(),
					minPointFilter: $('#MinPointFilterId').val(),
					maxPointFilter: $('#MaxPointFilterId').val(),
					minMoneyFilter: $('#MinMoneyFilterId').val(),
					maxMoneyFilter: $('#MaxMoneyFilterId').val(),
					statusFilter: $('#StatusFilterId').val()
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
                                    _viewPartnerRevenueModal.open({ id: data.record.partnerRevenue.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.partnerRevenue.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePartnerRevenue(data.record.partnerRevenue);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "partnerRevenue.userid",
						 name: "userid"   
					},
					{
						targets: 2,
						 data: "partnerRevenue.revenueType",
						 name: "revenueType"   ,
						render: function (revenueType) {
							return app.localize('Enum_RevenueTypeEnum_' + revenueType);
						}
			
					},
					{
						targets: 3,
						 data: "partnerRevenue.productId",
						 name: "productId"   
					},
					{
						targets: 4,
						 data: "partnerRevenue.point",
						 name: "point"   
					},
					{
						targets: 5,
						 data: "partnerRevenue.money",
						 name: "money"   
					},
					{
						targets: 6,
						 data: "partnerRevenue.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_RevenueStatusEnum_' + status);
						}
			
					}
            ]
        });

        function getPartnerRevenues() {
            dataTable.ajax.reload();
        }

        function deletePartnerRevenue(partnerRevenue) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _partnerRevenuesService.delete({
                            id: partnerRevenue.id
                        }).done(function () {
                            getPartnerRevenues(true);
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

        $('#CreateNewPartnerRevenueButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _partnerRevenuesService
                .getPartnerRevenuesToExcel({
				filter : $('#PartnerRevenuesTableFilter').val(),
					minUseridFilter: $('#MinUseridFilterId').val(),
					maxUseridFilter: $('#MaxUseridFilterId').val(),
					revenueTypeFilter: $('#RevenueTypeFilterId').val(),
					minPointFilter: $('#MinPointFilterId').val(),
					maxPointFilter: $('#MaxPointFilterId').val(),
					minMoneyFilter: $('#MinMoneyFilterId').val(),
					maxMoneyFilter: $('#MaxMoneyFilterId').val(),
					statusFilter: $('#StatusFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPartnerRevenueModalSaved', function () {
            getPartnerRevenues();
        });

		$('#GetPartnerRevenuesButton').click(function (e) {
            e.preventDefault();
            getPartnerRevenues();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPartnerRevenues();
		  }
		});
    });
})();