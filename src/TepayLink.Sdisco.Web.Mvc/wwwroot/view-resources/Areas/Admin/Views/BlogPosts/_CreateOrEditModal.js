(function ($) {
    app.modals.CreateOrEditBlogPostModal = function () {

        var _blogPostsService = abp.services.app.blogPosts;

        var _modalManager;
        var _$blogPostInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$blogPostInformationForm = _modalManager.getModal().find('form[name=BlogPostInformationsForm]');
            _$blogPostInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$blogPostInformationForm.valid()) {
                return;
            }

            var blogPost = _$blogPostInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _blogPostsService.createOrEdit(
				blogPost
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBlogPostModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);