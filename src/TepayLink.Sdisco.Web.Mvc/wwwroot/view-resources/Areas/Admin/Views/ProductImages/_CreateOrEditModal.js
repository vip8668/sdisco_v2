(function ($) {
    app.modals.CreateOrEditProductImageModal = function () {

        var _productImagesService = abp.services.app.productImages;

        var _modalManager;
        var _$productImageInformationForm = null;

		        var _ProductImageproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductImages/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductImages/_ProductImageProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productImageInformationForm = _modalManager.getModal().find('form[name=ProductImageInformationsForm]');
            _$productImageInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productImage = _$productImageInformationForm.serializeFormToObject();

            _ProductImageproductLookupTableModal.open({ id: productImage.productId, displayName: productImage.productName }, function (data) {
                _$productImageInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productImageInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productImageInformationForm.find('input[name=productName]').val(''); 
                _$productImageInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productImageInformationForm.valid()) {
                return;
            }
            if ($('#ProductImage_ProductId').prop('required') && $('#ProductImage_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var productImage = _$productImageInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productImagesService.createOrEdit(
				productImage
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductImageModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);