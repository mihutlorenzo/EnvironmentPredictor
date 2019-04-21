package com.example.sensorsapplication;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.v4.app.NotificationCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {
    private Button gatherTrainingDataButton;
    private Button buildModelButton;
    private Button deleteTrainingDataButton;
    private Button startPredictionButton;
    private Button stopPredictionButton;
   // private Button getPredictedValuesButton;

    ApiInterface apiService;
    Call<String> callBuildModel;
    Call<String> callDeleteTrainingData;
    Call<String> callStartPrediction;
    Call<String> callStopPrediction;
    Call<String> callGetPredictedValue;

    String predictedValue = null;
    Thread predictionThread;
    boolean running = false;

    private static final String TAG = MainActivity.class.getSimpleName();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        apiService = ApiClient.getClient();


        gatherTrainingDataButton = (Button) findViewById(R.id.gatherTrainingData);
        gatherTrainingDataButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openCollectTrainingDataActivity();
            }
        });

        buildModelButton = (Button) findViewById(R.id.buildModel);
        buildModelButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                buildModel();
            }
        });

        deleteTrainingDataButton = (Button) findViewById(R.id.deleteTrainingData);
        deleteTrainingDataButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deleteTrainingData();
            }
        });

        startPredictionButton = (Button) findViewById(R.id.startPredicting);
        startPredictionButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startPrediction();
            }
        });

        stopPredictionButton = (Button) findViewById(R.id.stopPredicting);
        stopPredictionButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                stopPrediction();
            }
        });

        //getPredictedValuesButton = (Button) findViewById(R.id.getPredicted);
        //getPredictedValuesButton.setOnClickListener(new View.OnClickListener() {
            //@Override
            //public void onClick(View v) {
             //   getPrediction();
            //}
        //});

        predictionThread = new Thread() {
            @Override
            public void run() {
                while(running){
                    try{
                        Thread.sleep(5000);

                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                callGetPredictedValue = apiService.GetPredictedValue();
                                callGetPredictedValue.enqueue(new Callback<String>() {
                                    @Override
                                    public void onResponse(Call<String>call, Response<String> response) {
                                        String messageFromServer = response.body();
                                        Log.d(TAG, "Predicted value have been retrieved: " + messageFromServer);
                                        //Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
                                        //Get an instance of NotificationManager//
                                        System.out.println(predictedValue);
                                        if(!messageFromServer.equals(predictedValue)){
                                            System.out.println("Condition"+messageFromServer.equals(predictedValue));
                                            predictedValue = messageFromServer;
                                            Notification.Builder mBuilder =
                                                    new Notification.Builder(MainActivity.this)
                                                            .setSmallIcon(R.mipmap.ic_launcher)
                                                            .setContentTitle("Predicted value changed!")
                                                            .setContentText(predictedValue);


                                            // Gets an instance of the NotificationManager service//

                                            NotificationManager mNotificationManager =

                                                    (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

                                            // When you issue multiple notifications about the same type of event,
                                            // it’s best practice for your app to try to update an existing notification
                                            // with this new information, rather than immediately creating a new notification.
                                            // If you want to update this notification at a later date, you need to assign it an ID.
                                            // You can then use this ID whenever you issue a subsequent notification.
                                            // If the previous notification is still visible, the system will update this existing notification,
                                            // rather than create a new one. In this example, the notification’s ID is 001//

                                            mNotificationManager.notify(001, mBuilder.build());
                                        }
                                    }
                                    @Override
                                    public void onFailure(Call<String>call, Throwable t) {
                                        // Log error here since request failed
                                        Log.e(TAG, t.toString());
                                    }
                                });
                            }
                        });
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            }
        };
    }
    public void openCollectTrainingDataActivity(){
        Intent intent = new Intent(this, CollectTrainingDataActivity.class);
        startActivity(intent);
    }

    public void buildModel() {
        callBuildModel = apiService.BuildModel();
        callBuildModel.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of building machine learning model stopped: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void deleteTrainingData() {
        callDeleteTrainingData = apiService.DeleteTrainingData();
        callDeleteTrainingData.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of deleting training data has been done: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void startPrediction() {
        callStartPrediction = apiService.StartPredictingOnTestData();
        callStartPrediction.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of prediction have been started: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
                predictionThread.start();
                running = true;
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    public void stopPrediction() {
        callStopPrediction = apiService.StopPredictingOnTestData();
        callStopPrediction.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Process of prediction have been stopped: " + messageFromServer);
                Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
                running = false;
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }

    /*ublic void getPrediction() {
        callGetPredictedValue = apiService.GetPredictedValue();
        callGetPredictedValue.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String>call, Response<String> response) {
                String messageFromServer = response.body();
                Log.d(TAG, "Predicted value have been retrieved: " + messageFromServer);
                //Toast.makeText(MainActivity.this, messageFromServer, Toast.LENGTH_LONG).show();
                //Get an instance of NotificationManager//

                Notification.Builder mBuilder =
                        new Notification.Builder(MainActivity.this)
                                .setSmallIcon(R.mipmap.ic_launcher)
                                .setContentTitle("Predicted value changed!")
                                .setContentText(messageFromServer);


                // Gets an instance of the NotificationManager service//

                NotificationManager mNotificationManager =

                        (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

                // When you issue multiple notifications about the same type of event,
                // it’s best practice for your app to try to update an existing notification
                // with this new information, rather than immediately creating a new notification.
                // If you want to update this notification at a later date, you need to assign it an ID.
                // You can then use this ID whenever you issue a subsequent notification.
                // If the previous notification is still visible, the system will update this existing notification,
                // rather than create a new one. In this example, the notification’s ID is 001//

                mNotificationManager.notify(001, mBuilder.build());
            }

            @Override
            public void onFailure(Call<String>call, Throwable t) {
                // Log error here since request failed
                Log.e(TAG, t.toString());
            }
        });
    }*/
}
