(() => {
  const words = new Map(Object.entries({
    'خانه':'Home','دوره‌ها':'Courses','دوره های آموزشی':'Courses','دوره‌ها و ویدیوها':'Courses & Videos',
    'فروشگاه':'Shop','نمونه‌کارها':'Portfolio','نمونه کارها':'Portfolio','سفارش':'Orders','سفارش‌ها':'Orders','سبد خرید':'Shopping cart',
    'مدیریت':'Admin','پروفایل':'Profile','خروج':'Sign out','ورود':'Sign in','ثبت‌نام':'Create account',
    'نام کاربری':'Username','شماره موبایل':'Mobile number','رمز عبور':'Password','تکرار رمز عبور':'Confirm password',
    'نام و نام خانوادگی':'Full name','ثبت‌نام و ارسال درخواست دسترسی':'Register and request access',
    'درباره مرزی':'About Marzi','دربارهٔ مرزی':'About Marzi','تماس با ما':'Contact us','تماس و گفتگو با مدیر':'Contact the artist',
    'دسترسی سریع':'Quick links','دسترسی سریع':'Quick links','داستان کارگاه':'Our story','خانه':'Home',
    'افزودن به سبد':'Add to cart','افزودن به سبد خرید':'Add to cart','ادامه خرید':'Continue shopping','تسویه حساب':'Checkout',
    'محصولات':'Products','قیمت':'Price','تعداد':'Quantity','جمع کل':'Total','حذف':'Remove','ثبت سفارش':'Place order',
    'سفارش ساخت':'Custom order','دوره‌های آموزشی':'Courses','همه':'View all','مشاهده سایت':'View site',
    'هیچ موردی یافت نشد':'No results found','در حال بارگذاری':'Loading','ذخیره':'Save','ویرایش':'Edit','لغو':'Cancel',
    'عنوان':'Title','توضیحات':'Description','جستجو':'Search','فیلتر':'Filter','وضعیت':'Status',
    'فعال':'Active','غیرفعال':'Inactive','در انتظار بررسی':'Pending review','ثبت شده':'Submitted',
    'پرداخت':'Payment','نشانی':'Address','استان':'Province','شهر':'City','کد پستی':'Postal code',
    'پیام':'Message','ارسال پیام':'Send message','نام':'Name','تلفن':'Phone','ایمیل':'Email',
    'ورود به Marzi Handicrafts':'Sign in to Marzi Handicrafts','ساخت حساب کاربری':'Create an account',
    'فراموشی رمز عبور':'Forgot password','مرا به خاطر بسپار':'Remember me','ارسال':'Submit',
    'دسته‌ها':'Categories','دسته بندی':'Category','مدیریت دسته‌بندی‌ها':'Manage categories',
    'ایجاد دسته':'Create category','عنوان جدید':'New title','ذخیره تغییرات':'Save changes',
    'آثار هنری دست‌ساز':'Handcrafted art','هنرهای دستی مازندران':'Handicrafts from Mazandaran'
  }));
  let english = false;
  const translate = root => {
    if (!english) return;
    const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT);
    const nodes = [];
    while (walker.nextNode()) nodes.push(walker.currentNode);
    for (const node of nodes) {
      const raw = node.nodeValue || '', key = raw.trim().replace(/\s+/g, ' ');
      if (words.has(key)) node.nodeValue = raw.replace(key, words.get(key));
    }
    root.querySelectorAll?.('[placeholder],[aria-label],[title]').forEach(el => {
      for (const attr of ['placeholder','aria-label','title']) {
        const value=el.getAttribute(attr); if (value && words.has(value.trim())) el.setAttribute(attr, words.get(value.trim()));
      }
    });
  };
  window.marziLanguage = {
    set(value) { const wasEnglish = english; english = value === 'en'; document.documentElement.lang=english?'en':'fa'; document.documentElement.dir=english?'ltr':'rtl'; if (english) translate(document.body); else if (wasEnglish) location.reload(); },
    translate: () => translate(document.body)
  };
  new MutationObserver(() => translate(document.body)).observe(document.documentElement,{childList:true,subtree:true});
})();
