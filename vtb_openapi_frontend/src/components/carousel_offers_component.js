"use client";
import Image from 'next/image';
import { Swiper, SwiperSlide } from 'swiper/react';
import { Pagination } from 'swiper/modules';
import 'swiper/swiper-bundle.css';
import { useEffect } from 'react';
import carouselStore from '../stores/carousel_store.js';


const Carousel = () => {
    const { slides, setSlides } = carouselStore();
  
    useEffect(() => {
        const fetchSlides = async () => {
          try {
            const response = await fetch('/carousel.json');
            const data = await response.json();
            setSlides(data);
          } catch (error) {
            console.error('Ошибка при получении данных:', error);
          }
        };
      
        fetchSlides();
      }, [setSlides]);
      
  
    if (!slides || slides.length === 0) {
      return <div className='text-center'>Загрузка...</div>;
    }
  
    return (
      <div className="relative w-full">
        <Swiper
          modules={[Pagination]}
          pagination={{ clickable: true }}
          className="w-full h-[500px] absolute"
        >
          {slides.map((slide, index) => (
            <SwiperSlide key={index} className="relative">
              <Image src={slide.image} alt={slide.title} layout="fill" className="w-full h-full object-cover" />
              <div className="absolute top-0 left-1/2 transform -translate-x-1/2 w-3/5 lg:w-2/5 pt-8 text-white">
                <h2 className="container text-3xl lg:text-4xl font-bold pb-5">Горячие предложения</h2>
                <div className="container rounded-md min-h-52 bg-custom-blur bg-opacity-70 p-4">
                  <h2 className="text-2xl lg:text-3xl">{slide.title}</h2>
                  <p>{slide.description}</p>
                </div>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    );
  };
  
  export default Carousel;
  