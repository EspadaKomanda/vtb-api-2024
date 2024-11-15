import StarRating from './star_rating.js';
import Image from 'next/image';


const ReviewItem = ({ review }) => {

    
    return (
            <li className="relative h-auto flex flex-col justify-end rounded bg-custom-bg-gray text-white p-4 mb-10">
                <div className='flex items-center flex-wrap mb-4'>
                    <Image 
                    src={review.author_avatar} 
                    alt="" 
                    width={60} 
                    height={60} 
                    className="rounded-full border-2 bg-custom-gradient"
                    />
                    <p className='mx-2 text-3xl font-semibold self-center'>{review.nickname}</p>
                    <StarRating rating={review.rating} editable={false} className="self-center"/>
                </div>
                <p className='text-xl'>{review.description}</p>
            </li>
    );
    
};

export default ReviewItem;
