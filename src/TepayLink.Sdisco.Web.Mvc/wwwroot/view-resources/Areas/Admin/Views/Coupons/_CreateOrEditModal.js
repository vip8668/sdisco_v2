(function ($) {
    app.modals.CreateOrEditCouponModal = function () {

        var _couponsService = abp.services.app.coupons;

        var _modalManager;
        var _$couponInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$couponInformationForm = _modalManager.getModal().find('form[name=CouponInformationsForm]');
            _$couponInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$couponInformationForm.valid()) {
                return;
            }

            var coupon = _$couponInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _couponsService.createOrEdit(
				coupon
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCouponModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);