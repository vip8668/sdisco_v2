(function () {
    $(function () {

        var _$blogPostsTable = $('#BlogPostsTable');
        var _blogPostsService = abp.services.app.blogPosts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.BlogPosts.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.BlogPosts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.BlogPosts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogPosts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BlogPosts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBlogPostModal'
        });

		 var _viewBlogPostModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogPosts/ViewblogPostModal',
            modalClass: 'ViewBlogPostModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$blogPostsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _blogPostsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BlogPostsTableFilter').val()
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
                                    _viewBlogPostModal.open({ id: data.record.blogPost.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.blogPost.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBlogPost(data.record.blogPost);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "blogPost.title",
						 name: "title"   
					},
					{
						targets: 2,
						 data: "blogPost.shortDescription",
						 name: "shortDescription"   
					},
					{
						targets: 3,
						 data: "blogPost.content",
						 name: "content"   
					},
					{
						targets: 4,
						 data: "blogPost.publishDate",
						 name: "publishDate" ,
					render: function (publishDate) {
						if (publishDate) {
							return moment(publishDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "blogPost.thumbImage",
						 name: "thumbImage"   
					},
					{
						targets: 6,
						 data: "blogPost.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_BlogStatusEnum_' + status);
						}
			
					}
            ]
        });

        function getBlogPosts() {
            dataTable.ajax.reload();
        }

        function deleteBlogPost(blogPost) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _blogPostsService.delete({
                            id: blogPost.id
                        }).done(function () {
                            getBlogPosts(true);
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

        $('#CreateNewBlogPostButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _blogPostsService
                .getBlogPostsToExcel({
				filter : $('#BlogPostsTableFilter').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBlogPostModalSaved', function () {
            getBlogPosts();
        });

		$('#GetBlogPostsButton').click(function (e) {
            e.preventDefault();
            getBlogPosts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBlogPosts();
		  }
		});
    });
})();