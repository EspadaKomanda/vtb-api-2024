"use client";
import { motion } from 'framer-motion';
import { useEffect, useState } from 'react';
import ExperienceItem from './experience_component.js';
import Pagination from './pagination_component.js';
import experienceStore from '@/stores/experience_store.js';


export default function WishListComponent() {

    const setExperiences = experienceStore((state) => state.setExperiences);
    const clearExperiences = experienceStore((state) => state.clearExperiences);
    const experiences = experienceStore((state) => state.experiences);
    const [totalPages, setTotalPages] = useState(1);
    const [currentPage, setCurrentPage] = useState(1);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchExperiences = async (page) => {
        try {
            setLoading(true);
            const totalPagesResponse = await fetch('/wish_list_json_files/total_pages.json');
            const totalPagesData = await totalPagesResponse.json();
            setTotalPages(totalPagesData.totalPages);

            const experiencesResponse = await fetch(`/wish_list_json_files/${page}_page.json`);
            const experiencesData = await experiencesResponse.json();
            setExperiences(experiencesData.experiences);
            
            
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

    if (loading) {
        return <div className='text-3xl text-white'>Загрузка...</div>;
    }

    if (error) {
        return <div className='text-3xl text-white'>Ошибка: {error}</div>;
    }

    return (
        <div>
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