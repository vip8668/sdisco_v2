(function ($) {
    app.modals.CreateOrEditProductReviewModal = function () {

        var _productReviewsService = abp.services.app.productReviews;

        var _modalManager;
        var _$productReviewInformationForm = null;

		        var _ProductReviewproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductReviews/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductReviews/_ProductReviewProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productReviewInformationForm = _modalManager.getModal().find('form[name=ProductReviewInformationsForm]');
            _$productReviewInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productReview = _$productReviewInformationForm.serializeFormToObject();

            _ProductReviewproductLookupTableModal.open({ id: productReview.productId, displayName: productReview.productName }, function (data) {
                _$productReviewInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productReviewInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productReviewInformationForm.find('input[name=productName]').val(''); 
                _$productReviewInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productReviewInformationForm.valid()) {
                return;
            }
            if ($('#ProductReview_ProductId').prop('required') && $('#ProductReview_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var productReview = _$productReviewInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productReviewsService.createOrEdit(
				productReview
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductReviewModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);