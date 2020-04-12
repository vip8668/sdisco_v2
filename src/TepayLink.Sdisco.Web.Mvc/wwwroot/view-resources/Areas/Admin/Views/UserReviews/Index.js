(function () {
    $(function () {

        var _$userReviewsTable = $('#UserReviewsTable');
        var _userReviewsService = abp.services.app.userReviews;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.UserReviews.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.UserReviews.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.UserReviews.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserReviews/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/UserReviews/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserReviewModal'
        });

		 var _viewUserReviewModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserReviews/ViewuserReviewModal',
            modalClass: 'ViewUserReviewModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$userReviewsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userReviewsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#UserReviewsTableFilter').val()
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
                                    _viewUserReviewModal.open({ id: data.record.userReview.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.userReview.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUserReview(data.record.userReview);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "userReview.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "userReview.reviewCount",
						 name: "reviewCount"   
					},
					{
						targets: 3,
						 data: "userReview.itineraty",
						 name: "itineraty"   
					},
					{
						targets: 4,
						 data: "userReview.service",
						 name: "service"   
					},
					{
						targets: 5,
						 data: "userReview.transport",
						 name: "transport"   
					},
					{
						targets: 6,
						 data: "userReview.guideTour",
						 name: "guideTour"   
					},
					{
						targets: 7,
						 data: "userReview.food",
						 name: "food"   
					},
					{
						targets: 8,
						 data: "userReview.rating",
						 name: "rating"   
					}
            ]
        });

        function getUserReviews() {
            dataTable.ajax.reload();
        }

        function deleteUserReview(userReview) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userReviewsService.delete({
                            id: userReview.id
                        }).done(function () {
                            getUserReviews(true);
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

        $('#CreateNewUserReviewButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _userReviewsService
                .getUserReviewsToExcel({
				filter : $('#UserReviewsTableFilter').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditUserReviewModalSaved', function () {
            getUserReviews();
        });

		$('#GetUserReviewsButton').click(function (e) {
            e.preventDefault();
            getUserReviews();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getUserReviews();
		  }
		});
    });
})();