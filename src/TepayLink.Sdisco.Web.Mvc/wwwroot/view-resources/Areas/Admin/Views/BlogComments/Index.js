(function () {
    $(function () {

        var _$blogCommentsTable = $('#BlogCommentsTable');
        var _blogCommentsService = abp.services.app.blogComments;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.BlogComments.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.BlogComments.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.BlogComments.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogComments/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BlogComments/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBlogCommentModal'
        });

		 var _viewBlogCommentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogComments/ViewblogCommentModal',
            modalClass: 'ViewBlogCommentModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$blogCommentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _blogCommentsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BlogCommentsTableFilter').val(),
					emailFilter: $('#EmailFilterId').val(),
					fullNameFilter: $('#FullNameFilterId').val(),
					blogPostTitleFilter: $('#BlogPostTitleFilterId').val()
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
                                    _viewBlogCommentModal.open({ id: data.record.blogComment.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.blogComment.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBlogComment(data.record.blogComment);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "blogComment.email",
						 name: "email"   
					},
					{
						targets: 2,
						 data: "blogComment.fullName",
						 name: "fullName"   
					},
					{
						targets: 3,
						 data: "blogComment.rating",
						 name: "rating"   
					},
					{
						targets: 4,
						 data: "blogComment.webSite",
						 name: "webSite"   
					},
					{
						targets: 5,
						 data: "blogComment.title",
						 name: "title"   
					},
					{
						targets: 6,
						 data: "blogComment.comment",
						 name: "comment"   
					},
					{
						targets: 7,
						 data: "blogPostTitle" ,
						 name: "blogPostFk.title" 
					}
            ]
        });

        function getBlogComments() {
            dataTable.ajax.reload();
        }

        function deleteBlogComment(blogComment) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _blogCommentsService.delete({
                            id: blogComment.id
                        }).done(function () {
                            getBlogComments(true);
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

        $('#CreateNewBlogCommentButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _blogCommentsService
                .getBlogCommentsToExcel({
				filter : $('#BlogCommentsTableFilter').val(),
					emailFilter: $('#EmailFilterId').val(),
					fullNameFilter: $('#FullNameFilterId').val(),
					blogPostTitleFilter: $('#BlogPostTitleFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBlogCommentModalSaved', function () {
            getBlogComments();
        });

		$('#GetBlogCommentsButton').click(function (e) {
            e.preventDefault();
            getBlogComments();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBlogComments();
		  }
		});
    });
})();