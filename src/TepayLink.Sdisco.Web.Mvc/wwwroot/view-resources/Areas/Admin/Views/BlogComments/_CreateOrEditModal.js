(function ($) {
    app.modals.CreateOrEditBlogCommentModal = function () {

        var _blogCommentsService = abp.services.app.blogComments;

        var _modalManager;
        var _$blogCommentInformationForm = null;

		        var _BlogCommentblogPostLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogComments/BlogPostLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BlogComments/_BlogCommentBlogPostLookupTableModal.js',
            modalClass: 'BlogPostLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$blogCommentInformationForm = _modalManager.getModal().find('form[name=BlogCommentInformationsForm]');
            _$blogCommentInformationForm.validate();
        };

		          $('#OpenBlogPostLookupTableButton').click(function () {

            var blogComment = _$blogCommentInformationForm.serializeFormToObject();

            _BlogCommentblogPostLookupTableModal.open({ id: blogComment.blogPostId, displayName: blogComment.blogPostTitle }, function (data) {
                _$blogCommentInformationForm.find('input[name=blogPostTitle]').val(data.displayName); 
                _$blogCommentInformationForm.find('input[name=blogPostId]').val(data.id); 
            });
        });
		
		$('#ClearBlogPostTitleButton').click(function () {
                _$blogCommentInformationForm.find('input[name=blogPostTitle]').val(''); 
                _$blogCommentInformationForm.find('input[name=blogPostId]').val(''); 
        });
		


        this.save = function () {
            if (!_$blogCommentInformationForm.valid()) {
                return;
            }
            if ($('#BlogComment_BlogPostId').prop('required') && $('#BlogComment_BlogPostId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('BlogPost')));
                return;
            }

            var blogComment = _$blogCommentInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _blogCommentsService.createOrEdit(
				blogComment
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBlogCommentModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);