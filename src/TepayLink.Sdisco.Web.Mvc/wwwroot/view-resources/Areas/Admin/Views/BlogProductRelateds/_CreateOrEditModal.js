(function ($) {
    app.modals.CreateOrEditBlogProductRelatedModal = function () {

        var _blogProductRelatedsService = abp.services.app.blogProductRelateds;

        var _modalManager;
        var _$blogProductRelatedInformationForm = null;

		        var _BlogProductRelatedblogPostLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogProductRelateds/BlogPostLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BlogProductRelateds/_BlogProductRelatedBlogPostLookupTableModal.js',
            modalClass: 'BlogPostLookupTableModal'
        });        var _BlogProductRelatedproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BlogProductRelateds/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BlogProductRelateds/_BlogProductRelatedProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$blogProductRelatedInformationForm = _modalManager.getModal().find('form[name=BlogProductRelatedInformationsForm]');
            _$blogProductRelatedInformationForm.validate();
        };

		          $('#OpenBlogPostLookupTableButton').click(function () {

            var blogProductRelated = _$blogProductRelatedInformationForm.serializeFormToObject();

            _BlogProductRelatedblogPostLookupTableModal.open({ id: blogProductRelated.blogPostId, displayName: blogProductRelated.blogPostTitle }, function (data) {
                _$blogProductRelatedInformationForm.find('input[name=blogPostTitle]').val(data.displayName); 
                _$blogProductRelatedInformationForm.find('input[name=blogPostId]').val(data.id); 
            });
        });
		
		$('#ClearBlogPostTitleButton').click(function () {
                _$blogProductRelatedInformationForm.find('input[name=blogPostTitle]').val(''); 
                _$blogProductRelatedInformationForm.find('input[name=blogPostId]').val(''); 
        });
		
        $('#OpenProductLookupTableButton').click(function () {

            var blogProductRelated = _$blogProductRelatedInformationForm.serializeFormToObject();

            _BlogProductRelatedproductLookupTableModal.open({ id: blogProductRelated.productId, displayName: blogProductRelated.productName }, function (data) {
                _$blogProductRelatedInformationForm.find('input[name=productName]').val(data.displayName); 
                _$blogProductRelatedInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$blogProductRelatedInformationForm.find('input[name=productName]').val(''); 
                _$blogProductRelatedInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$blogProductRelatedInformationForm.valid()) {
                return;
            }
            if ($('#BlogProductRelated_BlogPostId').prop('required') && $('#BlogProductRelated_BlogPostId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('BlogPost')));
                return;
            }
            if ($('#BlogProductRelated_ProductId').prop('required') && $('#BlogProductRelated_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var blogProductRelated = _$blogProductRelatedInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _blogProductRelatedsService.createOrEdit(
				blogProductRelated
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBlogProductRelatedModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);