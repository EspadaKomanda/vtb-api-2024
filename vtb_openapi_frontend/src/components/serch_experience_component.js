"use client";
import { useEffect, useState } from 'react';
import Image from 'next/image';
import * as img from '../assets/images.js';
import Slider from 'rc-slider';
import 'rc-slider/assets/index.css';
import '../styles/serch_experience_component_styles.css';
import StarRating from './star_rating.js';
import ExperienceItem from './experience_component.js'
import { motion, AnimatePresence } from 'framer-motion';
import Pagination from './pagination.js';
import experienceStore from '@/stores/experience_store.js';

class FilterData {
    constructor() {
        this.search = '';
        this.tours = false;
        this.experience = false;
        this.description = '';
        this.entertainment = false;
        this.dateFrom = '';
        this.dateTo = '';
        this.priceFrom = 0;
        this.priceTo = 0;
        this.distanceFrom = 0;
        this.distanceTo = 0;
        this.types = [];
        this.ratingFrom = 1;
        this.credit = false;
    }
}

export default function SearchTourComponent() {
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isFilterVisible, setIsFilterVisible] = useState(false);
    const [filters, setFilters] = useState(new FilterData());
    const setExperiences = experienceStore((state) => state.setExperiences);
    const clearExperiences = experienceStore((state) => state.clearExperiences);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const experiences = experienceStore((state) => state.experiences);
    const [categories, setCategories] = useState([]);

    const fetchExperiences = async (page) => {
        try {
            setLoading(true);
            const totalPagesResponse = await fetch('/serch_json_files/total_pages.json');
            const totalPagesData = await totalPagesResponse.json();
            setTotalPages(totalPagesData.totalPages);

            const experiencesResponse = await fetch(`/serch_json_files/${page}_page.json`);
            const experiencesData = await experiencesResponse.json();
            setExperiences(experiencesData.experiences);
            
            const response = await fetch('/serch_json_files/categories.json');
            const data = await response.json();
            setCategories(data);
            
        } catch (error) {
            setError('Ошибка при загрузке данных');
        } finally {
            setLoading(false);
        }
    };
    

    useEffect(() => {
        fetchExperiences(currentPage);
        window.scrollTo({ top: 0, behavior: 'smooth' });
        return () => clearExperiences(); 
    }, [currentPage]);
    

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    const toggleFilters = () => {
        setIsFilterVisible(!isFilterVisible);
    };

    const handleReset = () => {
        setFilters(new FilterData());
    };

    const handleSearch = (e) => {
        e.preventDefault();
        console.log(filters);
    };

    const handleDistanceChange = (value) => {
        setFilters({ ...filters, distanceFrom: value[0], distanceTo: value[1] });
    };

    const handlePriceChange = (value) => {
        setFilters({ ...filters, priceFrom: value[0], priceTo: value[1] });
    };

    const handleCategoryChange = (e) => {
        const { value, checked } = e.target;
        if (checked) {
            setFilters((prev) => ({
                ...prev,
                types: [...prev.types, value],
            }));
        } else {
            setFilters((prev) => ({
                ...prev,
                types: prev.types.filter((type) => type !== value),
            }));
        }
    };

    if (loading) {
        return <div className='text-3xl text-white'>Загрузка...</div>;
    }

    if (error) {
        return <div className='text-3xl text-white'>Ошибка: {error}</div>;
    }

    return (
        <div className="p-4 pt-20">
            <h2 className="text-white text-5xl font-bold">Поиск по турам и развлечениям</h2>
            <form onSubmit={handleSearch} className='relative'>
                <input 
                    type="text" 
                    placeholder="Введите название тура" 
                    className="mt-4 mb-5 p-2 w-full rounded bg-custom-bg-gray text-white outline-none"
                    value={filters.search}
                    onChange={(e) => setFilters({ ...filters, search: e.target.value })}
                />
                <button 
                    type="button" 
                    onClick={toggleFilters} 
                    className="sm:right-28 right-20 top-6 absolute transition duration-300 hover:scale-105 active:scale-95"
                >
                    <Image src={img.filter} alt="filter" width={25} height={25} className='ml-auto' />
                </button>
                <AnimatePresence>
                {isFilterVisible && (
                    <motion.div
                    initial={{ opacity: 0, y: -10 }}
                    animate={{ opacity: isFilterVisible ? 1 : 0, y: isFilterVisible ? 0 : -10 }}
                    exit={{ opacity: 0, y: -10 }}
                    transition={{ duration: 0.3 }}
                    className={`right-3 mt-4 px-8 py-4 z-10 bg-custom-bg-gray rounded lg:w-1/2 flex flex-col gap-y-2 absolute text-2xl`}
                    >
                        <div className="flex justify-end items-center">
                            <Image 
                                src={img.exit} 
                                alt="close" 
                                width={30}
                                height={30} 
                                onClick={toggleFilters} 
                                className='transition duration-300 hover:scale-110 active:scale-95' 
                            />
                        </div>

                        <div className='grid grid-cols-2'>
                            <label className="text-white">Туры:</label>
                            <div className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    id="tours" 
                                    className="hidden" 
                                    checked={filters.tours} 
                                    onChange={(e) => setFilters({ ...filters, tours: e.target.checked })}
                                />
                                <label 
                                    htmlFor="tours" 
                                    className="flex items-center cursor-pointer text-white">
                                    <span className={`w-5 h-5 inline-block mr-2 rounded-sm border-4 border-custom-border
                                        ${filters.tours ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                        transition duration-200`}>
                                        {filters.tours && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                    </span>
                                </label>
                            </div>
                        </div>

                        <div className='grid grid-cols-2'>
                            <label className="text-white">Развлечения:</label>
                            <div className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    id="entertainment" 
                                    className="hidden" 
                                    checked={filters.entertainment} 
                                    onChange={(e) => setFilters({ ...filters, entertainment: e.target.checked })}
                                />
                                <label 
                                    htmlFor="entertainment" 
                                    className="flex items-center cursor-pointer text-white">
                                    <span className={`w-5 h-5 inline-block mr-2 rounded-sm border-4 border-custom-border
                                        ${filters.entertainment ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                        transition duration-200`}>
                                        {filters.entertainment && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                    </span>
                                </label>
                            </div>
                        </div>

                        <div className='grid grid-cols-2'>
                            <label className="text-white">Дата от:</label>
                            <input 
                                type="date" 
                                className="mt-1 px-2 py-1 rounded bg-custom-blur text-white"
                                value={filters.dateFrom || ''}
                                onChange={(e) => setFilters({ ...filters, dateFrom: e.target.value })}
                            />
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Дата до:</label>
                            <input 
                                type="date" 
                                className="mt-1 px-2 py-1 rounded bg-custom-blur text-white"
                                value={filters.dateTo || ''}
                                onChange={(e) => setFilters({ ...filters, dateTo: e.target.value })}
                            />
                        </div>
                        <div className='grid grid-cols-2 '>
                            <label className="text-white">Стоимость:</label>
                            <div className="flex flex-col">
                                <Slider
                                    range
                                    min={0}
                                    max={100000}
                                    value={[filters.priceFrom || 10000, filters.priceTo || 50000]}
                                    onChange={handlePriceChange}
                                    className="my-2 custom-slider"
                                />
                                <div className="flex justify-between">
                                    <input 
                                        type="number" 
                                        className="mt-1 mr-5 px-2 rounded bg-custom-blur text-white w-1/2"
                                        value={filters.priceFrom || 10000}
                                        onChange={(e) => setFilters({ ...filters, priceFrom: e.target.value })}
                                    />
                                    <input 
                                        type="number" 
                                        className="mt-1 px-2 rounded bg-custom-blur text-white w-1/2"
                                        value={filters.priceTo || 50000}
                                        onChange={(e) => setFilters({ ...filters, priceTo: e.target.value })}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className='grid grid-cols-2 '>
                            <label className="text-white">Расстояние:</label>
                            <div className="flex flex-col">
                                <Slider
                                    range
                                    min={0}
                                    max={100000}
                                    value={[filters.distanceFrom || 10000, filters.distanceTo || 50000]}
                                    onChange={handleDistanceChange}
                                    className="my-2 custom-slider"
                                />
                                <div className="flex justify-between">
                                    <input 
                                        type="number" 
                                        className="mt-1 px-2 mr-5 rounded bg-custom-blur text-white w-1/2"
                                        value={filters.distanceFrom || 10000}
                                        onChange={(e) => setFilters({ ...filters, distanceFrom: e.target.value })}
                                    />
                                    <input 
                                        type="number" 
                                        className="mt-1 px-2  rounded bg-custom-blur text-white w-1/2"
                                        value={filters.distanceTo || 50000}
                                        onChange={(e) => setFilters({ ...filters, distanceTo: e.target.value })}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Рейтинг от:</label>
                            <StarRating rating={filters.ratingFrom}  setRating={(value) => setFilters({ ...filters, ratingFrom: value })} />
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Кредит:</label>
                            <div className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    id="credit" 
                                    className="hidden" 
                                    checked={filters.credit} 
                                    onChange={(e) => setFilters({ ...filters, credit: e.target.checked })}
                                />
                                <label 
                                    htmlFor="credit" 
                                    className="flex items-center cursor-pointer text-white">
                                    <span className={`w-5 h-5 inline-block mr-2 rounded-sm border-4 border-custom-border
                                        ${filters.credit ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                        transition duration-200`}>
                                        {filters.credit && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                    </span>
                                </label>
                            </div>
                        </div>
                        <div className='grid grid-cols-2'>
                            <label className="text-white">Категории:</label>
                            <div className="mt-1">
                                {categories.map((category, index) => (
                                    <div key={index} className="flex items-center">
                                        <input
                                            type="checkbox"
                                            id={`category-${index}`}
                                            value={category}
                                            checked={filters.types.includes(category)}
                                            onChange={handleCategoryChange}
                                            className="hidden"
                                        />
                                        <label 
                                            htmlFor={`category-${index}`} 
                                            className="flex items-center cursor-pointer text-white">
                                            <span className={`w-5 h-5 inline-block mr-2 rounded-sm border-4 border-custom-border
                                                ${filters.types.includes(category) ? 'bg-custom-gradient' : 'bg-custom-blur'} 
                                                transition duration-200`}>
                                                {filters.types.includes(category) && <span className="block w-full h-full bg-custom-gradient rounded-sm"></span>}
                                            </span>
                                            {category}
                                        </label>
                                    </div>
                                ))}
                            </div>
                        </div>


                        <button 
                            type="button" 
                            onClick={handleReset} 
                            className="mt-2 bg-custom-blur text-white px-4 py-2 rounded text-3xl"
                        >
                            Сбросить
                        </button>
                    </motion.div>
                )}
                </AnimatePresence>
                <button 
                    type="submit" 
                    className="bg-custom-gradient text-white py-1 px-2 sm:px-5 rounded right-1 top-5 absolute transition duration-300 hover:scale-105 active:scale-95"
                >
                    Найти
                </button>

            </form>

            <div className="mt-4">
                <motion.div
                initial={{ opacity: 0, y: -5  }}
                animate={{ opacity:  1,  y: 0 }}
                transition={{ duration: 0.3, ease: 'easeInOut' }}
                >
                    <ul className="mt-2">
                        {Array.isArray(experiences) && experiences.length > 0 ? (
                            experiences.map((experience) => (
                                <ExperienceItem key={experience.id} experience={experience} />
                            ))
                        ) : (
                            <li className='text-3xl text-white'>Ничего не найдено...</li>
                        )}
                    </ul>
                </motion.div>
            </div>
            <Pagination totalPages={totalPages} currentPage={currentPage} onPageChange={handlePageChange} />
        </div>
    );
}

