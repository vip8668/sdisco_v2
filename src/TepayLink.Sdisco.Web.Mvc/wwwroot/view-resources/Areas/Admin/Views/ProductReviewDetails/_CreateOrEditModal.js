(function ($) {
    app.modals.CreateOrEditProductReviewDetailModal = function () {

        var _productReviewDetailsService = abp.services.app.productReviewDetails;

        var _modalManager;
        var _$productReviewDetailInformationForm = null;

		        var _ProductReviewDetailproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductReviewDetails/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductReviewDetails/_ProductReviewDetailProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productReviewDetailInformationForm = _modalManager.getModal().find('form[name=ProductReviewDetailInformationsForm]');
            _$productReviewDetailInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productReviewDetail = _$productReviewDetailInformationForm.serializeFormToObject();

            _ProductReviewDetailproductLookupTableModal.open({ id: productReviewDetail.productId, displayName: productReviewDetail.productName }, function (data) {
                _$productReviewDetailInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productReviewDetailInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productReviewDetailInformationForm.find('input[name=productName]').val(''); 
                _$productReviewDetailInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productReviewDetailInformationForm.valid()) {
                return;
            }
            if ($('#ProductReviewDetail_ProductId').prop('required') && $('#ProductReviewDetail_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var productReviewDetail = _$productReviewDetailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productReviewDetailsService.createOrEdit(
				productReviewDetail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductReviewDetailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);