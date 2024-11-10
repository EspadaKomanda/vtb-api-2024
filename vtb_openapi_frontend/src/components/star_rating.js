import Image from "next/image";
import * as img from '../assets/images.js';

const StarRating = ({ rating, setRating, editable = true }) => {
    const stars = [1, 2, 3, 4, 5];
    const filledStar = img.star_active;
    const emptyStar = img.star;

    const roundedRating = editable ? rating : Math.round(rating);

    return (
        <div className="flex">
            {stars.map((star) => (
                <Image
                    key={star}
                    src={star <= roundedRating ? filledStar : emptyStar}
                    alt={`${star} star`}
                    className={`w-6 h-6 mr-2 ${editable ? 'cursor-pointer' : ''}`}
                    onClick={editable ? () => setRating(star) : undefined}
                />
            ))}
        </div>
    );
};

export default StarRating;
