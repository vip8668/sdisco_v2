(function () {
    $(function () {

        var _$userReviewDetailsTable = $('#UserReviewDetailsTable');
        var _userReviewDetailsService = abp.services.app.userReviewDetails;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.UserReviewDetails.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.UserReviewDetails.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.UserReviewDetails.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserReviewDetails/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/UserReviewDetails/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserReviewDetailModal'
        });

		 var _viewUserReviewDetailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserReviewDetails/ViewuserReviewDetailModal',
            modalClass: 'ViewUserReviewDetailModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$userReviewDetailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userReviewDetailsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#UserReviewDetailsTableFilter').val()
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
                                    _viewUserReviewDetailModal.open({ id: data.record.userReviewDetail.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.userReviewDetail.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUserReviewDetail(data.record.userReviewDetail);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "userReviewDetail.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "userReviewDetail.itineraty",
						 name: "itineraty"   
					},
					{
						targets: 3,
						 data: "userReviewDetail.service",
						 name: "service"   
					},
					{
						targets: 4,
						 data: "userReviewDetail.transport",
						 name: "transport"   
					},
					{
						targets: 5,
						 data: "userReviewDetail.guideTour",
						 name: "guideTour"   
					},
					{
						targets: 6,
						 data: "userReviewDetail.food",
						 name: "food"   
					},
					{
						targets: 7,
						 data: "userReviewDetail.rating",
						 name: "rating"   
					},
					{
						targets: 8,
						 data: "userReviewDetail.title",
						 name: "title"   
					},
					{
						targets: 9,
						 data: "userReviewDetail.comment",
						 name: "comment"   
					}
            ]
        });

        function getUserReviewDetails() {
            dataTable.ajax.reload();
        }

        function deleteUserReviewDetail(userReviewDetail) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userReviewDetailsService.delete({
                            id: userReviewDetail.id
                        }).done(function () {
                            getUserReviewDetails(true);
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

        $('#CreateNewUserReviewDetailButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _userReviewDetailsService
                .getUserReviewDetailsToExcel({
				filter : $('#UserReviewDetailsTableFilter').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditUserReviewDetailModalSaved', function () {
            getUserReviewDetails();
        });

		$('#GetUserReviewDetailsButton').click(function (e) {
            e.preventDefault();
            getUserReviewDetails();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getUserReviewDetails();
		  }
		});
    });
})();