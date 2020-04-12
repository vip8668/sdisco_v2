(function () {
    $(function () {

        var _$couponsTable = $('#CouponsTable');
        var _couponsService = abp.services.app.coupons;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Coupons.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Coupons.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Coupons.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Coupons/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Coupons/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCouponModal'
        });

		 var _viewCouponModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Coupons/ViewcouponModal',
            modalClass: 'ViewCouponModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$couponsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _couponsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CouponsTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
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
                                    _viewCouponModal.open({ id: data.record.coupon.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.coupon.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCoupon(data.record.coupon);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "coupon.code",
						 name: "code"   
					},
					{
						targets: 2,
						 data: "coupon.amount",
						 name: "amount"   
					},
					{
						targets: 3,
						 data: "coupon.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_CouponStatusEnum_' + status);
						}
			
					}
            ]
        });

        function getCoupons() {
            dataTable.ajax.reload();
        }

        function deleteCoupon(coupon) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _couponsService.delete({
                            id: coupon.id
                        }).done(function () {
                            getCoupons(true);
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

        $('#CreateNewCouponButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _couponsService
                .getCouponsToExcel({
				filter : $('#CouponsTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					minAmountFilter: $('#MinAmountFilterId').val(),
					maxAmountFilter: $('#MaxAmountFilterId').val(),
					statusFilter: $('#StatusFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCouponModalSaved', function () {
            getCoupons();
        });

		$('#GetCouponsButton').click(function (e) {
            e.preventDefault();
            getCoupons();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCoupons();
		  }
		});
    });
})();