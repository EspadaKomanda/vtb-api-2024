package ru.espada.vtb_demo_front.service;

import jakarta.annotation.PostConstruct;
import lombok.SneakyThrows;
import org.springframework.stereotype.Service;
import ru.espada.vtb_demo_front.models.PayData;
import ru.espada.vtb_demo_front.models.ReviewData;
import ru.espada.vtb_demo_front.models.ServiceModel;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Random;

@Service
public class ServiceService {

    private List<ServiceModel> services = new ArrayList<>();
    private int PAGE_SIZE = 3;

    @PostConstruct
    public void init() {
        for (int i = 0; i < 20; i++) {
            services.add(getService(i));
        }
    }

    public ServiceModel getServiceById(int id) {
        return services.stream().filter(service -> service.getId() == id).findFirst().orElse(null);
    }

    public List<ServiceModel> getServices(int page) {
        int totalPages = (int) Math.ceil((double) services.size() / PAGE_SIZE);
        int adjustedPage = page % totalPages;

        return services.stream()
                .skip((long) PAGE_SIZE * adjustedPage)
                .limit(PAGE_SIZE)
                .toList();
    }

    @SneakyThrows
    public List<ServiceModel> getRecommendedServices(int tourId, int page) {
        Thread.sleep(1000);
        System.out.println(page);
        return services.stream().filter(service -> service.getId() % 3 == 0).skip((long) PAGE_SIZE * page).limit(PAGE_SIZE).toList();
    }

    private ServiceModel getService(int id) {
        Random random = new Random();
        String name = "Развлечение " + random.nextInt(100);
        String image = "https://avatars.mds.yandex.net/get-altay/248099/2a0000015e9f061499e2f143acd3dc7e6a77/XXL_height";
        String description = "Тестовое описание " + random.nextInt(100);
        String shortDescription = "Тестовое описание " + random.nextInt(100);

        List<String> categories = new ArrayList<>();
        categories.add("Тестовая категория " + random.nextInt(100));
        categories.add("Тестовая категория " + random.nextInt(100));

        int price = random.nextInt(10000);

        List<ReviewData> reviews = new ArrayList<>();
        String date = new SimpleDateFormat("dd.MM.yyyy").format(new Date());
        reviews.add(new ReviewData(0, "Тестовое имя", image, "Тестовое описание", date, random.nextInt(5)));

        int rating = 5;

        List<PayData> payVariants = new ArrayList<>();
        for (String payName : new String[]{"Со страхованием", "Без страхования"}) {
            List<String> variants = new ArrayList<>();
            variants.add("Сразу - " + random.nextInt(100000) + " руб.");
            variants.add("В кредит - " + random.nextInt(100000) + " руб.");
            payVariants.add(new PayData(payName, variants));
        }

        return new ServiceModel(id, name, image, description, shortDescription, categories, price, reviews, rating, payVariants);
    }

    @SneakyThrows
    public List<ServiceModel> getFavorites() {
        Thread.sleep(1000);
        return services.stream().filter(service -> service.getId() % 3 == 0).toList();
    }
}
