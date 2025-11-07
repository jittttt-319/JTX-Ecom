# Assets Folder Guide

## Image Folder Structure

- `wwwroot/assets/images/` - Main images folder
- `wwwroot/assets/images/concerts/` - Concert event images
- `wwwroot/assets/images/artists/` - Artist profile images (optional)
- `wwwroot/assets/images/banners/` - Homepage banner images

## Recommended Images to Add

### Hero Banner
- `banner-hero.jpg` - Main homepage banner (1920x800px recommended)
- Use high-quality concert crowd or stage images

### Concert Events
Place concert promotional images in `concerts/` folder:
- `concert1.jpg`
- `concert2.jpg`
- `concert3.jpg`
- etc.

## Usage in Views

To reference images in your Razor views:
```cshtml
<img src="~/assets/images/concerts/concert1.jpg" alt="Concert Name" />
```

## Tips
- Use optimized images (WebP format recommended for better performance)
- Recommended dimensions: 
  - Hero banners: 1920x800px
  - Concert cards: 600x400px
  - Thumbnails: 300x200px
