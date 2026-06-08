# 2D Spawner Package

![Unity](https://img.shields.io/badge/Unity-6000.0.30f1%2B-black?logo=unity)
![License](https://img.shields.io/github/license/AbolfazlHo/Spawner)
![GitHub last commit](https://img.shields.io/github/last-commit/AbolfazlHo/Spawner)
![Version](https://img.shields.io/badge/version-0.2.0-blue)

یک بسته (Package) قدرتمند و انعطاف‌پذیر برای Spawner دوبعدی در یونیتی (Unity) که برای ساده‌سازی فرآیند Spawning آبجکت‌های دوبعدی با ویژگی‌های پیشرفته‌ای مانند Spawning خودکار، ایمنی در برخورد و قرارگیری بر اساس Grid طراحی شده است.

---

## 📖 فهرست مطالب

* [ویژگی‌ها](#-ویژگی‌ها)
* [شروع سریع](#-شروع-سریع)
* [نصب](#-نصب)
* [نحوه استفاده](#-نحوه-استفاده)
    * [تنظیمات اولیه](#1-تنظیمات-اولیه)
    * [Spawning خودکار](#2-spawning-خودکار)
    * [قرارگیری ایمن در برابر برخورد](#3-قرارگیری-ایمن-در-برابر-برخورد)
    * [قرارگیری بر اساس گرید](#4-قرارگیری-بر-اساس-گرید)
    * [Object Pooling](#5-object-pooling)
    * [رویدادها](#6-رویدادها)
    * [کنترل انتخاب Prefab](#7-کنترل-انتخاب-prefab)

* [مرجع API](#-مرجع-api)
    * [Spawner](#spawner-monobehaviour) 
    * [Spawnable](#spawnable)
    * [Automation](#automation)
    * [Limitation](#limitation)
    * [CollisionSafety](#collisionsafety)
    * [GridPlacement](#gridplacement)
    * [یکپارچه‌سازی با Inspector](#یکپارچه‌سازی-با-inspector)
    * [نمونه کاربرد](#نمونه-کاربرد)
    
* [مشارکت‌ها](#-مشارکت‌ها)
* [مجوز (License)](#-مجوز-license)

## 🚀 ویژگی‌ها

* **Spawning خودکار**: به راحتی آبجکت‌ها را برای Spawning خودکار با فواصل زمانی قابل تنظیم، پیکربندی کنید.
* **کنترل‌های محدودیت**: برای Spawning خودکار، محدودیت‌هایی را بر اساس زمان یا تعداد تنظیم کنید.
* **قرارگیری ایمن در برابر برخورد**: اطمینان حاصل می‌کند که آبجکت‌های Spawn شده با Colliderهای موجود همپوشانی نداشته باشند و در صورت لزوم یک فضای خالی پیدا می‌کند.
* **قرارگیری بر اساس گرید**: آبجکت‌ها را به آسانی در یک ساختار گرید از پیش تعریف شده قرار می‌دهد.
* **مبتنی بر رویداد (Event-Driven)**: از `Unity Events` برای فراخوانی‌های انعطاف‌پذیر در شروع Spawn، پایان Spawn و رسیدن به محدودیت، استفاده کنید.
* **بسیار قابل تنظیم**: پارامترهای کلیدی را در Inspector نمایش می‌دهد تا راه‌اندازی آسان و بدون نیاز به کدنویسی باشد.
* **Object Pooling**: از یک پول آبجکت بسیار کارآمد برای استفاده مجدد از آبجکت‌های Spawn شده استفاده می‌کند که سربار ایجاد (instantiation overhead) را به شدت کاهش داده و عملکرد را بهبود می‌بخشد.
**کنترل انتخاب Prefab**: به آسانی بین انتخاب تصادفی Prefabها از لیست یا Spawning آن‌ها به صورت ترتیبی جابجا شوید.

## 📦 نصب

### پیش‌نیازها

این بسته برای عملیات‌های ناهمگام (asynchronous) به [UniTask](https://github.com/Cysharp/UniTask) وابسته است. لطفاً اطمینان حاصل کنید که **قبل از** نصب این بسته Spawner دوبعدی، UniTask در پروژه یونیتی شما نصب شده باشد.
ابتدا UniTask را از مخزن GitHub آن به پروژه خود اضافه کنید.

### نصب 2D Spawner

#### نصب از طریق URL گیت با استفاده از Unity Package Manager
برای نصب `2D Spawner` از طریق URL گیت با استفاده از Unity Package Manager، مراحل زیر را دنبال کنید:
 1. پروژه یونیتی خود را باز کنید.
 2. به `Window > Package Manager` بروید.
 3. روی آیکون `+` در گوشه بالا سمت چپ پنجره Package Manager کلیک کنید.
 4. `Add package from git URL ...` را انتخاب کنید.
 5. URL زیر را Paste کرده و روی "Add" کلیک کنید:
 ```
https://github.com/AbolfazlHo/Spawner.git?path=Assets/ir.soor.spawner2d
```
 
 ## ⚡ شروع سریع
  
 1.  `UniTask` را نصب کنید.
 2.  بسته `2D Spawner` را از طریق URL گیت نصب کنید.
 3.  یک `GameObject` خالی (مثلاً `MySpawner`) در صحنه خود بسازید.
 4.  کامپوننت `Spawner` را به آن اضافه کنید.
 5.  Prefab‌های دوبعدی مورد نظر خود را به لیست Spawnables اختصاص دهید. همچنین می‌توانید گزینه Choose Spawnable Randomly را فعال یا غیرفعال کنید تا نحوه انتخاب Prefabها (تصادفی یا ترتیبی) مشخص شود.
 6.  یک `GameObject` برای ناحیه Spawn (همراه با یک `Collider2D`) اضافه کرده و آن را در کامپوننت `Spawner` اختصاص دهید.
 7.  `Spawn Automatically` را فعال کنید، یا به صورت دستی با کد `Spawn()` را فراخوانی کنید.
  

## 🛠 نحوه استفاده

### 1. تنظیمات اولیه

1.  یک `GameObject` خالی در صحنه خود بسازید (مثلاً `MySpawner`).
2.  کامپوننت `Spawner` را به آن اضافه کنید.
3.  Prefa‌های دوبعدی که می‌خواهید Spawn شوند را به لیست `Spawnables` اختصاص دهید.
4.  یک `Spawn Area GameObject` (مثلاً یک Quad با `Collider2D`) برای کنترل محل Spawning آبجکت‌ها تعریف کنید.

### 2. Spawning خودکار

1.  در کامپوننت `Spawner`، گزینه `Spawn Automatically` را فعال کنید.
2.  منوی `Spawn Automation Settings` را باز کنید.
3.  `Per Spawn Interval` را تنظیم کنید (تأخیر بین هر Spawn).
4.  اگر `Stop Spawning Automatically` فعال است، `Limitation Settings` را پیکربندی کنید:
    * `Limitation Type` را انتخاب کنید (Count یا Time).
    * `Limit Count By` (مثلاً ۱۰ آبجکت) یا `Limit Time By` (مثلاً ۶۰ ثانیه) را تنظیم کنید.

### 3. قرارگیری ایمن در برابر برخورد

1.  `Is Collision Safe` را در کامپوننت `Spawner` فعال کنید.
2.  منوی `Collision Safety Settings` را باز کنید.
3.  اطمینان حاصل کنید که Prefa‌های `Spawnable` شما کامپوننت‌های `Collider2D` مناسبی دارند. سیستم سعی می‌کند در صورت اشغال بودن موقعیت اولیه، یک نقطه خالی پیدا کند.

### 4. قرارگیری بر اساس گرید

1.  در کامپوننت `Spawner`، گزینه `Use Object Pool` را فعال کنید.
2.  منوی `Pooler Settings` را باز کنید.
3.  کامپوننت به شکل خودکار `Prefab`های مناسب را به لیست `Objects to Pool` می‌افزاید.

### 5. استفاده از Object Pooling

1.  در `Collision Safety Settings`، `Is Grid Placement` را فعال کنید.
2.  منوی `Grid Placement Settings` را باز کنید.
3.  `Spawnable Size` (اندازه پایه آبجکت‌های Spawn شده) را تنظیم کنید.
4.  `Padding` را برای افزودن فاصله بین خانه‌های گرید، تنظیم کنید.
5.  `Rows Count` یا `Columns Count` را برای تعریف ابعاد گرید خود پیکربندی کنید. سیستم به صورت خودکار اندازه‌ی خانه‌ها را محاسبه می‌کند.
    * اگر `Columns Count` بزرگ‌تر از ۰ باشد، ردیف و ستون بر اساس آن محاسبه می‌شوند.
    * در غیر این صورت، ردیف و ستون بر اساس `Rows Count` محاسبه می‌شوند.

### 6. رویدادها

 `Automation` و `Limitation`، `UnityEvents` را نمایش می‌دهند که می‌توانید برای منطق سفارشی خود به آن‌ها متصل شوید:
    
* **Automation**:
    * `On Spawn Start Event`: هنگام شروع Spawning خودکار فراخوانی می‌شود.
    * `On Spawn End Event`: هنگام پایان Spawning خودکار (چه به صورت طبیعی و چه با توقف) فراخوانی می‌شود.
    
* **Limitation**:
    * `On Spawn Start Event`: توسط `Automation` هنگام شروع Spawning فراخوانی می‌شود (اگر Spawning خودکار محدود شده باشد).
    * `On Limitation Reached Event`: هنگام رسیدن به تعداد یا محدودیت زمانی تعریف شده برای Spawn، فراخوانی می‌شود.
    * `On Spawn End Event`: توسط `Automation` هنگام پایان Spawning فراخوانی می‌شود (اگر Spawning خودکار محدود شده باشد).
    
## 🧩 مرجع API

این مرجع API یک دید کلی کامل از ساختار، Propertyها و رفتار بسته `Spawner2D` ارائه می‌دهد.

### `Spawner` (`MonoBehaviour`)
کلاس اصلی برای کنترل فرآیند Spawning آبجکت‌های بازی در صحنه.

#### متدهای عمومی
-   `void Spawn()`: بلافاصله یک آبجکت را بر اساس تنظیمات فعلی Spawn می‌کند.
-   `void StopSpawning()`: فرآیند Spawning خودکار را متوقف می‌کند (اگر فعال باشد).

#### Propertyهای Inspector
-   **Spawn Area GameObject**: `GameObject`ی که ناحیه Spawning را تعریف می‌کند. معمولاً شامل یک `BoxCollider2D` است.
-   **Spawnables**: لیستی از آبجکت‌هایی که می‌توانند Spawn شوند.
-   **Spawn Automatically**: تعیین می‌کند که آیا فرآیند Spawn به صورت خودکار شروع شود یا خیر.
-   **Spawn Automation Settings**: تنظیمات مربوط به Spawning خودکار.
-   **Is Collision Safe**: بررسی‌های ایمنی در برابر برخورد را در طول Spawning فعال می‌کند.
-   **Collision Safety Settings**: تنظیمات مربوط به ایمنی در برابر برخورد (مانند بررسی فاصله یا گرید).
-   **Spawnable Tag**: یک Tag سفارشی برای شناسایی آبجکت‌های قابل Spawn.
-   **Use Object Pool**: تعیین می‌کند آیا `Spawner` از پول آبجکت برای مدیریت کارآمد آبجکت‌ها استفاده کند یا خیر.
-   **Pooler Settings**:  تنظیمات مربوط به پول آبجکت، از جمله `Prefab`هایی که قرار است در پول قرار گیرند.


### `Spawnable`

Marker script to indicate that an object is spawnable.
### `Spawnable`
یک اسکریپت نشانگر (Marker) برای تعیین اینکه یک آبجکت قابل Spawn شدن است.

### `Automation`
زمان‌بندی و شرایط شروع/پایان Spawning خودکار را کنترل می‌کند.

#### Propertyها
-   **Per Spawn Interval**: بازه زمانی بین هر Spawn.
-   **Stop Spawning Automatically**: تعیین می‌کند که آیا فرآیند Spawn به صورت خودکار متوقف شود یا خیر.
-   **Limitation Settings**: محدودیت‌هایی که فرآیند Spawn را متوقف می‌کنند (بر اساس تعداد یا زمان).
-   **Choose Spawnable Randomly**: یک bool که اگر true باشد، برای هر Spawn یک Prefab تصادفی انتخاب می‌شود؛ در غیر این صورت، Prefabها به صورت ترتیبی از لیست _spawnables انتخاب می‌شوند.
-   **On Spawn Start Event**: رویدادی که در شروع Spawning تریگر می‌شود.
-   **On Spawn End Event**: رویدادی که در پایان Spawning تریگر می‌شود.

> **توجه**: `Limitation Settings` تنها زمانی فعال و استفاده می‌شوند که `Stop Spawning Automatically` فعال باشد.

### `Limitation`
محدودیت‌ها را برای توقف فرآیند Spawning خودکار مدیریت می‌کند.

#### Enum

```csharp
LimitationType { Time, Count }
```

### Limitation
Manages constraints for stopping the automatic spawning process.

#### Properties

- **Limitation Type**: نوع محدودیت (زمان یا تعداد).

- **Limit Time By**: برای حالت زمان: تعداد ثانیه‌هایی که پس از آن spawning متوقف می‌شود.

- **Limit Count By**: برای حالت تعداد: تعداد spawningهایی که پس از آن spawning متوقف می‌شود.

- **On Spawn Start Event**

- **On Limitation Reached Event**

- **On Spawn End Event**

### `CollisionSafety`

تنظیمات ایمنی در برابر برخورد برای جلوگیری از همپوشانی آبجکت‌های Spawn شده.

#### Properties

- **Is Placement**: ایمنی اولیه در برابر برخورد بر اساس فاصله را فعال می‌کند.

- **Is Grid Placement**: قرارگیری بر اساس گرید را فعال می‌کند.

- **Grid Placement Settings**: تنظیمات مربوط به قرارگیری بر اساس گرید.

### `GridPlacement`

پیکربندی طرح‌بندی بر اساس گرید.

#### Propertyها

- **Spawnable Size**: اندازه پایه هر آبجکت قابل Spawn.

- **Padding**: فضای اضافی بین هر خانه گرید.

- **Rows Count**: تعداد ردیف‌ها (مقدار 1- برای محاسبه خودکار).

- **Columns Count**: تعداد ستون‌ها (مقدار 1- برای محاسبه خودکار).


### یکپارچه‌سازی با Inspector

#### Custom Editors

کلاس‌های `PropertyDrawer` برای `Automation`، `Limitation` و `CollisionSafety` پیاده‌سازی شده‌اند تا تجربه کاربری در Inspector را بهبود بخشند. نمایش Propertyها به صورت شرطی بر اساس تنظیمات انجام می‌شود.

#### ویرایشگر اصلی: `SpawnerInspector`

رندر کننده Inspector سفارشی برای `Spawner` با طرح‌بندی بصری و کنترل‌های گروه‌بندی شده شامل:

- Spawn Settings
- Automation
- Collision Safety
- Events

### Example Usage

```csharp
[SerializeField] private Spawner _spawner;

void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        _spawner.Spawn();
    }

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
        _spawner.StopSpawning();
    }
}
```

## 🤝 مشارکت‌ها

از مشارکت‌های شما استقبال می‌شود! اگر باگ (bug) یا درخواست ویژگی جدیدی دارید، لطفاً یک issue در [مخزن گیت‌هاب](https://github.com/AbolfazlHo/Spawner/issues) باز کنید.

## 📄 مجوز (License)

این بسته تحت مجوز MIT توزیع شده است. برای اطلاعات بیشتر، به [فایل LICENSE](https://raw.githubusercontent.com/AbolfazlHo/Spawner/refs/heads/main/LICENSE) مراجعه کنید.

