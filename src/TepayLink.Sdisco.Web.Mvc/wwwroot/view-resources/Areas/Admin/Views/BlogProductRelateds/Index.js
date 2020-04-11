(function () {
    $(function () {

        var _$blogProductRelatedsTable = $('#BlogProductRelatedsTable');
        var _blogProductRelatedsService = abp.services.app.blogProductRelateds;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.BlogProductRelateds.Create'),
            edit: abp.auth.hasPermission('Pages.BlogProductRelateds.Edit'),
            'delete': abp.auth.hasPermission('Pages.BlogProductRelateds.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogProductRelateds/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BlogProductRelateds/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBlogProductRelatedModal'
        });

		 var _viewBlogProductRelatedModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogProductRelateds/ViewblogProductRelatedModal',
            modalClass: 'ViewBlogProductRelatedModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$blogProductRelatedsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _blogProductRelatedsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BlogProductRelatedsTableFilter').val(),
					blogPostTitleFilter: $('#BlogPostTitleFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
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
                                    _viewBlogProductRelatedModal.open({ id: data.record.blogProductRelated.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.blogProductRelated.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBlogProductRelated(data.record.blogProductRelated);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "blogPostTitle" ,
						 name: "blogPostFk.title" 
					},
					{
						targets: 2,
						 data: "productName" ,
						 name: "productFk.name" 
					}
            ]
        });

        function getBlogProductRelateds() {
            dataTable.ajax.reload();
        }

        function deleteBlogProductRelated(blogProductRelated) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _blogProductRelatedsService.delete({
                            id: blogProductRelated.id
                        }).done(function () {
                            getBlogProductRelateds(true);
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

        $('#CreateNewBlogProductRelatedButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _blogProductRelatedsService
                .getBlogProductRelatedsToExcel({
				filter : $('#BlogProductRelatedsTableFilter').val(),
					blogPostTitleFilter: $('#BlogPostTitleFilterId').val(),
					productNameFilter: $('#ProductNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBlogProductRelatedModalSaved', function () {
            getBlogProductRelateds();
        });

		$('#GetBlogProductRelatedsButton').click(function (e) {
            e.preventDefault();
            getBlogProductRelateds();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBlogProductRelateds();
		  }
		});
    });
})();