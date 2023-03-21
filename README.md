# CodeFormer-Runner

front-end app for CodeFormer (AI Face Restoration) https://github.com/sczhou/CodeFormer

Note: Envairoment name must be 'codeformer' as in the installation guide in the git above
```
# create new anaconda env
conda create -n codeformer python=3.8 -y
conda activate codeformer
```



First of all click on the first dir to select the python app execute file.

![image](https://user-images.githubusercontent.com/7548709/226600175-0386c95d-6a09-4761-934f-8fa3235d6165.png)

Next go to your CodeFormer folder and select the "inference_codeformer.py" execulte file.

![image](https://user-images.githubusercontent.com/7548709/226597150-f5ff646c-f564-4e18-86da-d868d4245b0d.png)


Next select folder with pictures ( can select any, in my example its the images folder from the CodeFormer ).

![image](https://user-images.githubusercontent.com/7548709/226597477-7f31e63c-c5e5-4949-b7f0-0c861f0d4abe.png)


Pictures will be loaded.


![image](https://user-images.githubusercontent.com/7548709/226614971-23cb1b2b-9629-4fd9-9083-8270f44ce55b.png)



Next select folder for the output pictures ( can be any, in my example its the results folder in the CodeFormer )


![image](https://user-images.githubusercontent.com/7548709/226598010-495f45e0-3a4d-481c-9f24-f4079dbd12d1.png)


Now click on some images to select tham and click on Start, the first 2 images selected in the example.
( Note: you can modify weight and upscale parameters for each image )

![image](https://user-images.githubusercontent.com/7548709/226598413-d61de989-1c13-4e0d-b3a5-af3437fa9afb.png)


Process started wait some time depending on the number of images you selected.


![image](https://user-images.githubusercontent.com/7548709/226599199-f202c70d-6759-46f6-a8f7-dfbba369207e.png)


Process finished, check for the results in your output folder.



![image](https://user-images.githubusercontent.com/7548709/226599451-d7647339-ae8f-4769-afd1-836800646076.png)
