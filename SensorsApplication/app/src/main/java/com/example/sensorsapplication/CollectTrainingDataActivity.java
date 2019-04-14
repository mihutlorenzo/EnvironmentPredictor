package com.example.sensorsapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.RadioButton;
import android.widget.Toast;
import android.widget.ToggleButton;

public class CollectTrainingDataActivity extends AppCompatActivity {
    private ToggleButton togglebutton;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_collect_training_data);
        togglebutton = (ToggleButton) findViewById(R.id.togglebutton);
    }

    public void onRadioButtonClicked(View view) {
        // Is the button now checked?
        boolean checked = ((RadioButton) view).isChecked();

        // Check which radio button was clicked
        switch(view.getId()) {
            case R.id.light:
                if (checked)
                    // Pirates are the best
                    break;
            case R.id.dark:
                if (checked)
                    // Ninjas rule
                    break;
            case R.id.wet:
                if (checked)
                    // Ninjas rule
                    break;
            case R.id.dry:
                if (checked)
                    // Ninjas rule
                    break;
            case R.id.hot:
                if (checked)
                    // Ninjas rule
                    break;
            case R.id.cold:
                if (checked)
                    // Ninjas rule
                    break;
            case R.id.noice:
                if (checked)
                    // Ninjas rule
                    break;
            case R.id.quiet:
                if (checked)
                    // Ninjas rule
                    break;
        }
    }

    public void toggleClick(View v){
        if(togglebutton.isChecked()) {
            togglebutton.setText("Stop Collecting Training Data");
        }
        else{
            togglebutton.setText("Collect Training Data");
        }
    }

}
