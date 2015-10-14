climbingFish = (function ($, cFish) {

    // 1. ECMA-262/5
    'use strict';

    var cfg = {
        cache: {
            greyScaleSection: '.greyscaleSection',
            errorPageSection: '.ErrorPageSection',
            cssBackground: 'background',
            section: 'section',
            nav: 'nav',
            mainnav: '#SiteNavigation',
            mainnavitem: '.mainNav',
            target: '[data-target]',
            navItem: '[data-navItem]',
            scrollTop: '.ScrollTop'

        },
        event: {
            click: 'click',
            change: 'change',
            load: 'load',
            scroll: 'scroll'
        },
        data: {
            background: "background",
            target: "target",
            navItem: "navItem"
        }
    }

    cFish.HomePage = {

        init: function () {
            this.cacheItems();
            this.bindEvents();
        },

        cacheItems: function () {
            var cache = cfg.cache;
            this.greyScaleSection = $(cache.greyScaleSection);
            this.section = $(cache.section);
            this.nav = $(cache.nav);
            this.mainnav = $(cache.mainnav);
            this.mainnavitem = $(cache.mainnavitem);
            this.target = $(cache.target);
            this.navItem = $(cache.navItem);
        },

        bindEvents: function () {
            var self = this,
                mainnav = $(cfg.cache.mainnav),
                nav = $(cfg.cache.nav),
                navHeight = nav.outerHeight(),
                sitenavHeight = mainnav.outerHeight();

            $(window).load(function (e) {
                self.setHomepageBackgrounds();
                self.setErrorPageBackground();
            });

            $(document).on(cfg.event.click, cfg.cache.target, function (e) {
                var target = $(this).data(cfg.data.target);
                var elem = $('#' + target);
                $('html, body').animate({
                    scrollTop: $(elem).offset().top - navHeight - sitenavHeight
                }, 1500, "easeInOutBack");
            });

        },

        setHomepageBackgrounds: function () {
            $(cfg.cache.greyScaleSection).each(function () {
                var background = $(this).data(cfg.data.background);
                $(this).css(cfg.cache.cssBackground, "url(" + background + ")");
            });
        },
        setErrorPageBackground: function () {
            $(cfg.cache.errorPageSection).each(function () {
                var background = $(this).data(cfg.data.background);
                $(this).css(cfg.cache.cssBackground, "url(" + background + ")");
            });
        }
    };
    return cFish;

}(window.jQuery, window.cFish || {}));
