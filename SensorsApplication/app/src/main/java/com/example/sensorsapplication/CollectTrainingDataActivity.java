package com.example.sensorsapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;
import android.widget.ToggleButton;

public class CollectTrainingDataActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_collect_training_data);
        ((RadioGroup) findViewById(R.id.toggleGroup)).setOnCheckedChangeListener(ToggleListener);
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
            default:
                break;
        }
    }

    static final RadioGroup.OnCheckedChangeListener ToggleListener = new RadioGroup.OnCheckedChangeListener() {
        @Override
        public void onCheckedChanged(final RadioGroup radioGroup, final int i) {
            for (int j = 0; j < radioGroup.getChildCount(); j++) {
                final ToggleButton view = (ToggleButton) radioGroup.getChildAt(j);
                view.setChecked(view.getId() == i);
            }
        }
    };

    public void onToggle(View view) {
        ((RadioGroup)view.getParent()).check(view.getId());
        boolean checked = ((ToggleButton) view).isChecked();

        // Check which radio button was clicked
        switch(view.getId()) {
            case R.id.startGatheringTrainingData:
                if (checked)
                    // Pirates are the best
                    break;
            case R.id.stopGatheringTrainingData:
                if (checked)
                    // Ninjas rule
                    break;
            default:
                break;
        }
    }


}
